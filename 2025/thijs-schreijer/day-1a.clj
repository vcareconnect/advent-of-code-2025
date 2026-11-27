; https://adventofcode.com/2025/day/1

; AI generated from the Lua version

(require '[clojure.string :as str])

(let [input (-> "thijs-schreijer/day-1a-input.txt" slurp str/split-lines)
      {:keys [zero]}
      (reduce
        (fn [{:keys [dial zero]} line]
          (let [direction (subs line 0 1)
                step-size (Long/parseLong (subs line 1))
                step-size (case direction
                            "L" (- step-size)
                            "R" step-size
                            (throw (ex-info (str "Unknown direction: " direction) {:line line})))
                dial' (mod (+ dial step-size) 100)]
            {:dial dial'
             :zero (if (zero? dial') (inc zero) zero)}))
        {:dial 50 :zero 0}
        input)]
  (println "entry code:" zero))
