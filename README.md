# JA.Interval
An `Interval` structure defined by an upper and lower limit, with open or closed end and limits can be finite or infinite.

## JA.Interval.Demo

Console outpuit from the demo

```
Test Ranges
Interval                                  | Contains()           | Is()
              name                  value |      0     -1      1 | single  range    all    pos    neg  pos|0  neg|0 closed   open
------------------------------------------+----------------------+---------------------------------------------------------------
               all                 (-∞,∞) |   True   True   True |  False   True   True  False  False  False  False  False  False
          negative                 (-∞,0) |  False   True  False |  False   True  False  False   True  False   True  False  False
          positive                  (0,∞) |  False  False   True |  False   True  False   True  False   True  False  False  False
  negative or zero                 (-∞,0] |   True   True  False |  False   True  False  False  False  False   True  False  False
  positive or zero                  [0,∞) |   True  False   True |  False   True  False  False  False   True  False  False  False
              zero                  [0,0] |   True  False  False |   True  False  False  False  False   True   True   True  False
            finite   [-1.8e+308,1.8e+308] |   True   True   True |  False   True  False  False  False  False  False   True  False
           nothing                     () |  False  False  False |  False  False  False  False  False   True   True  False   True
           invalid                (invld) |  False  False  False |  False  False  False  False  False  False  False   True  False
               one                  [1,1] |  False  False   True |   True  False  False   True  False   True  False   True  False
            closed                 [-1,1] |   True   True   True |  False   True  False  False  False  False  False   True  False
              open                 (-1,1) |   True  False  False |  False   True  False  False  False  False  False  False   True
              upto                (-∞,-1] |  False   True  False |  False   True  False  False   True  False   True  False  False
              from                  [1,∞) |  False  False   True |  False   True  False   True  False   True  False  False  False
        close-open                 [-1,1) |   True   True  False |  False   True  False  False  False  False  False  False  False
        open-close                 (-1,1] |   True  False   True |  False   True  False  False  False  False  False  False  False


Test Combinations
Intersect   |  (-∞,∞)   (-∞,0)    (0,∞)    [1,1]   [-1,1]   (-1,1)  (-∞,-1]    [1,∞)
------------+------------------------------------------------------------------------
(-∞,∞)      |  (-∞,∞)   (-∞,0)    (0,∞)    [1,1]   [-1,1]   (-1,1)  (-∞,-1]    [1,∞)
(-∞,0)      |  (-∞,0)   (-∞,0)       ()  (invld)   [-1,0)   (-1,0)  (-∞,-1]  (invld)
(0,∞)       |   (0,∞)       ()    (0,∞)    [1,1]    (0,1]    (0,1)  (invld)    [1,∞)
[1,1]       |   [1,1]  (invld)    [1,1]    [1,1]    [1,1]    [1,1]  (invld)    [1,1]
[-1,1]      |  [-1,1]   [-1,0)    (0,1]    [1,1]   [-1,1]   [-1,1]  [-1,-1]    [1,1]
(-1,1)      |  (-1,1)   (-1,0)    (0,1)    [1,1)   [-1,1)   (-1,1)  (-1,-1]    [1,1)
(-∞,-1]     | (-∞,-1]  (-∞,-1]  (invld)  (invld)  [-1,-1]  (-1,-1]  (-∞,-1]  (invld)
[1,∞)       |   [1,∞)  (invld)    [1,∞)    [1,1]    [1,1]    [1,1)  (invld)    [1,∞)

Union       |  (-∞,∞)   (-∞,0)    (0,∞)    [1,1]   [-1,1]   (-1,1)  (-∞,-1]    [1,∞)
------------+------------------------------------------------------------------------
(-∞,∞)      |  (-∞,∞)   (-∞,∞)   (-∞,∞)   (-∞,∞)   (-∞,∞)   (-∞,∞)   (-∞,∞)   (-∞,∞)
(-∞,0)      |  (-∞,∞)   (-∞,0)   (-∞,∞)   (-∞,1]   (-∞,1]   (-∞,1)   (-∞,0)   (-∞,∞)
(0,∞)       |  (-∞,∞)   (-∞,∞)    (0,∞)    (0,∞)   [-1,∞)   (-1,∞)   (-∞,∞)    (0,∞)
[1,1]       |  (-∞,∞)   (-∞,1]    (0,∞)    [1,1]   [-1,1]   (-1,1]   (-∞,1]    [1,∞)
[-1,1]      |  (-∞,∞)   (-∞,1]   [-1,∞)   [-1,1]   [-1,1]   (-1,1]   (-∞,1]   [-1,∞)
(-1,1)      |  (-∞,∞)   (-∞,1)   (-1,∞)   (-1,1]   (-1,1]   (-1,1)   (-∞,1)   (-1,∞)
(-∞,-1]     |  (-∞,∞)   (-∞,0)   (-∞,∞)   (-∞,1]   (-∞,1]   (-∞,1)  (-∞,-1]   (-∞,∞)
[1,∞)       |  (-∞,∞)   (-∞,∞)    (0,∞)    [1,∞)   [-1,∞)   (-1,∞)   (-∞,∞)    [1,∞)

divide span = [0,100] into 5 segments
segment(1) = [0,20)
segment(2) = [20,40)
segment(3) = [40,60)
segment(4) = [60,80)
segment(5) = [80,100]

split span = [0,100] into (30, 45, 75)
segment(1) = [0,30)
segment(2) = [30,45)
segment(3) = [45,75)
segment(4) = [75,100]
```
