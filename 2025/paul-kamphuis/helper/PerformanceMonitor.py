from dataclasses import dataclass, field
import time
import tracemalloc
from typing import Callable, ClassVar, Dict, Optional

class PerformanceMonitorError(Exception):
    """A custom exception used to report errors in use of PerformanceMonitor class"""

@dataclass
class PerformanceMonitor:
    timers: ClassVar[Dict[str, float]] = {}
    name: Optional[str] = None
    text: str = "Elapsed time: {:0.4f} seconds"
    mem_text: str = "Current memory usage is {:0.4f}MB; Peak was {:0.4f}MB"
    logger: Optional[Callable[[str], None]] = print
    _start_time: Optional[float] = field(default=None, init=False, repr=False)

    def __post_init__(self) -> None:
        """Add timer to dict of timers after initialization"""
        if self.name is not None:
            self.timers.setdefault(self.name, 0)

    def __enter__(self):
        """Start a new timer as a context manager"""
        self.start()
        return self

    def __exit__(self, *exc_info):
        """Stop the context manager timer"""
        self.stop()

    def start(self) -> None:
        """Start a new timer"""
        if self._start_time is not None:
            raise PerformanceMonitorError(f"Timer is running. Use .stop() to stop it")

        self._start_time = time.perf_counter()
        tracemalloc.start()        


    def stop(self) -> float:
        """Stop the timer, and report the elapsed time"""
        if self._start_time is None:
            raise PerformanceMonitorError(f"Timer is not running. Use .start() to start it")

        # Calculate elapsed time
        elapsed_time = time.perf_counter() - self._start_time
        self._start_time = None
        memory = tracemalloc.get_traced_memory()
        tracemalloc.stop()

        # Report elapsed time
        if self.logger:
            self.logger(self.text.format(elapsed_time))
            self.logger(self.mem_text.format(memory[0] / 10**6, memory[1] / 10**6))
        if self.name:
            self.timers[self.name] += elapsed_time

        return elapsed_time