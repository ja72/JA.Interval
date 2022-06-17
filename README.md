# JA.Interval
An `Interval` structure defined by an upper and lower limit, with open or closed end and limits can be finite or infinite.

    public enum LimitType
    {
        Closed,
        Open,
        Infinite
    }

    public readonly struct Interval : IEquatable<Interval>
    {
        public LimitType LowerLimit { get; }
        public LimitType UpperLimit { get; }
        public double LowerValue { get; }
        public double UpperValue { get; }

        public static readonly Interval All;
        public static readonly Interval Negative;
        public static readonly Interval Positive;
        public static readonly Interval NegativeOrZero;
        public static readonly Interval PositiveOrZero;
        public static readonly Interval Zero;
        public static readonly Interval Nothing;
        public static readonly Interval Invalid;
        public static readonly Interval DoubleDomain;
        public static readonly Interval FinitePositive;
        public static readonly Interval FiniteNegative;

        public static Interval Value(double x);
        public static Interval ClosedRange(double lower, double upper);
        public static Interval OpenRange(double lower, double upper);
        public static Interval NegativeInfinityTo(double x);
        public static Interval ToPositiveInfinity(double x);
        public static Interval OpenClosed(double lower, double upper);
        public static Interval CloseOpen(double lower, double upper);

        public static implicit operator Interval(double x);
        
        public bool IsAll { get => }
        public bool IsFinite { get => }
        public bool IsZero { get => }
        public bool IsNothing { get => }
        public bool IsSingular { get => }
        public bool IsRange { get => }
        public bool IsClosed { get => }
        public bool IsOpen { get => }
        public bool IsPositive { get => }
        public bool IsNegative { get => }
        public bool IsPositiveOrZero { get => }
        public bool IsNegativeOrZero { get => }
        public bool IsLowerValid { get => }
        public bool IsUpperValid { get => }
        public bool IsValid { get => }

        public bool Contains(double x);

        public IEnumerable<Interval> Divide(int segments);
        public IEnumerable<Interval> Split(params double[] values);

        public static Interval Offset(Interval a, double delta);
        public static Interval Scale(double factor, Interval a);
        public static Interval Negate(Interval a);

        public static Interval operator -(Interval a) =>
        public static Interval operator +(Interval a, double x) =>
        public static Interval operator +(double x, Interval a) =>
        public static Interval operator -(Interval a, double x) =>
        public static Interval operator -(double x, Interval a) =>
        public static Interval operator *(Interval a, double x) =>
        public static Interval operator *(double x, Interval a)  =>
        public static Interval operator |(Interval a, Interval b)  =>
        public static Interval operator &(Interval a, Interval b)  =>

        public static Interval Union(Interval a, Interval b);        
        public static Interval Intersect(Interval a, Interval b);
        public string ToString(string formatting, IFormatProvider provider);
		public bool Equals(Interval other);
    }


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
