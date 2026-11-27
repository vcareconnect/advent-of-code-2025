-- https://adventofcode.com/2025/day/1

local pl_utils = require("pl.utils")

local input = assert(pl_utils.readlines("thijs-schreijer/day-1a-input.txt"))

local dial_position = 50
local zero_count = 0

for i, line in ipairs(input) do
    local direction = line:sub(1, 1)
    local step_size = tonumber(line:sub(2, -1))

    if direction == "L" then
        step_size = -step_size
    elseif direction ~= "R" then
        -- sanity check
        error("Unknown direction: " .. direction)
    end

    dial_position = (dial_position + step_size) % 100
    if dial_position == 0 then
        zero_count = zero_count + 1
    end
end

print("entry code:", zero_count)
