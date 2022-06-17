using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace JA
{
    public enum LimitType
    {
        Closed,
        Open,
        Infinite
    }

    public readonly struct Interval : IEquatable<Interval>
    {
        readonly double lower_x;
        readonly double upper_x;

        #region Factory
        Interval(double lowerValue, double upperValue, LimitType lowerLimit, LimitType upperLimit)
            : this()
        {
            this.lower_x = lowerValue;
            this.upper_x = upperValue;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            if (UpperValue < LowerValue)
            {
                this = new Interval(upperValue, lowerValue, upperLimit, lowerLimit);
            }
        }
        public static readonly Interval All = new Interval(0, 0, LimitType.Infinite, LimitType.Infinite);
        public static readonly Interval Negative = new Interval(0, 0, LimitType.Infinite, LimitType.Open);
        public static readonly Interval Positive = new Interval(0, 0, LimitType.Open, LimitType.Infinite);
        public static readonly Interval NegativeOrZero = new Interval(0, 0, LimitType.Infinite, LimitType.Closed);
        public static readonly Interval PositiveOrZero = new Interval(0, 0, LimitType.Closed, LimitType.Infinite);
        public static readonly Interval Zero = Value(0);
        public static readonly Interval Nothing = new Interval(0, 0, LimitType.Open, LimitType.Open);
        public static readonly Interval Invalid = new Interval(double.NegativeInfinity, double.PositiveInfinity, LimitType.Closed, LimitType.Closed);
        public static readonly Interval DoubleDomain = ClosedRange(double.MinValue, double.MaxValue);
        public static readonly Interval FinitePositive = ClosedRange(double.Epsilon, double.MaxValue);
        public static readonly Interval FiniteNegative = ClosedRange(double.NegativeInfinity, double.Epsilon);

        public static Interval Value(double x) => new Interval(x, x, LimitType.Closed, LimitType.Closed);
        public static Interval ClosedRange(double lower, double upper) => new Interval(lower, upper, LimitType.Closed, LimitType.Closed);
        public static Interval OpenRange(double lower, double upper) => new Interval(lower, upper, LimitType.Open, LimitType.Open);
        public static Interval NegativeInfinityTo(double x) => new Interval(0, x, LimitType.Infinite, LimitType.Closed);
        public static Interval ToPositiveInfinity(double x) => new Interval(x, 0, LimitType.Closed, LimitType.Infinite);
        public static Interval OpenClosed(double lower, double upper) => new Interval(lower, upper, LimitType.Open, LimitType.Closed);
        public static Interval CloseOpen(double lower, double upper) => new Interval(lower, upper, LimitType.Closed, LimitType.Open);

        public static implicit operator Interval(double x) => Value(x);
        #endregion

        #region Properties
        public (double lower, double upper) GetLimits() => (LowerValue, UpperValue);

        public LimitType LowerLimit { get; }
        public LimitType UpperLimit { get; }

        public double LowerValue
        {
            get => LowerLimit == LimitType.Infinite ? double.NegativeInfinity : lower_x;
        }
        public double UpperValue
        {
            get => UpperLimit == LimitType.Infinite ? double.PositiveInfinity : upper_x;
        }
        public bool IsAll { get => this == All; }
        /// <summary>
        /// Check if interval has finite limits.
        /// </summary>
        public bool IsFinite { get => LowerLimit != LimitType.Infinite && UpperLimit != LimitType.Infinite; }
        public bool IsZero { get => this == Zero; }
        public bool IsNothing { get => lower_x == upper_x && LowerLimit == LimitType.Open && UpperLimit == LimitType.Open; }
        public bool IsSingular { get => upper_x == lower_x && UpperLimit == LimitType.Closed && LowerLimit == LimitType.Closed; }
        public bool IsRange { get => IsValid && UpperValue > LowerValue; }
        public bool IsClosed { get => LowerLimit == LimitType.Closed && UpperLimit == LimitType.Closed; }
        public bool IsOpen { get => LowerLimit == LimitType.Open && UpperLimit == LimitType.Open; }
        public bool IsPositive { get => !IsNothing && LowerValue >= 0 && LowerLimit == LimitType.Open || LowerValue > 0 && LowerLimit == LimitType.Closed; }
        public bool IsNegative { get => !IsNothing && UpperValue <= 0 && UpperLimit == LimitType.Open || UpperValue < 0 && UpperLimit == LimitType.Closed; }
        public bool IsPositiveOrZero { get => LowerValue >= 0; }
        public bool IsNegativeOrZero { get => UpperValue <= 0; }
        public bool IsLowerValid
        {
            get => LowerLimit == LimitType.Infinite || lower_x > double.NegativeInfinity;
        }
        public bool IsUpperValid
        {
            get => UpperLimit == LimitType.Infinite || upper_x < double.PositiveInfinity;
        }
        public bool IsValid { get => IsLowerValid && IsUpperValid; }

        bool IsAbove(double x)
        {
            switch (LowerLimit)
            {
                case LimitType.Closed:
                    return x >= lower_x;
                case LimitType.Open:
                    return x > lower_x;
                case LimitType.Infinite:
                    return x > double.NegativeInfinity;
                default:
                    throw new NotSupportedException();
            }
        }
        bool IsBelow(double x)
        {
            switch (UpperLimit)
            {
                case LimitType.Closed:
                    return x <= upper_x;
                case LimitType.Open:
                    return x < upper_x;
                case LimitType.Infinite:
                    return x < double.PositiveInfinity;
                default:
                    throw new NotSupportedException();
            }
        }
        public bool Contains(double x)
        {
            return IsValid && IsAbove(x) && IsBelow(x);
        }

        public IEnumerable<Interval> Divide(int segments)
        {
            if (IsFinite)
            {
                if (IsRange)
                {
                    double lx = lower_x;
                    var lt = LowerLimit;
                    double ux = upper_x;
                    var ut = UpperLimit;
                    double dx = (ux - lx) / segments;
                    for (int i = 0; i < segments; i++)
                    {
                        if (i == 0)
                        {
                            yield return new Interval(lx, lx + dx, lt, LimitType.Open);
                        }
                        else if (i == segments - 1)
                        {
                            yield return new Interval(ux - dx, ux, LimitType.Closed, ut);
                        }
                        else
                        {
                            yield return new Interval(lx + i * dx, lx + (i + 1) * dx, LimitType.Closed, LimitType.Open);
                        }
                    }
                }
            }
        }

        public IEnumerable<Interval> Split(params double[] values)
        {
            if (IsRange)
            {
                double lx = lower_x;
                var lt = LowerLimit;
                for (int i = 0; i < values.Length; i++)
                {
                    double x = values[i];
                    yield return new Interval(lx, x, lt, LimitType.Open);
                    lx = x;
                    lt = LimitType.Closed;
                }
                double ux = upper_x;
                var ut = UpperLimit;
                yield return new Interval(lx, ux, lt, ut);
            }
        }

        public void Deconstruct(out double ll, out double ul)
        {
            ll = LowerValue;
            ul = UpperValue;
        }

        #endregion

        #region Algebra
        public static Interval Offset(Interval a, double delta)
            => new Interval(a.lower_x + delta, a.upper_x + delta, a.LowerLimit, a.UpperLimit);
        public static Interval Scale(double factor, Interval a)
            => new Interval(a.lower_x * factor, a.upper_x * factor, a.LowerLimit, a.UpperLimit);
        public static Interval Negate(Interval a)
            => new Interval(-a.upper_x, -a.lower_x, a.UpperLimit, a.LowerLimit);
        #endregion

        #region Operators
        public static Interval operator -(Interval a) => Negate(a);

        public static Interval operator +(Interval a, double x)
            => Offset(a, x);
        public static Interval operator +(double x, Interval a)
            => Offset(a, x);
        public static Interval operator -(Interval a, double x)
            => Offset(a, -x);
        public static Interval operator -(double x, Interval a)
            => Offset(-a, x);
        public static Interval operator *(Interval a, double x)
            => Scale(x, a);
        public static Interval operator *(double x, Interval a)
            => Scale(x, a);
        public static Interval operator |(Interval a, Interval b)
            => Union(a, b);
        public static Interval operator &(Interval a, Interval b)
            => Intersect(a, b);
        #endregion

        #region Set Operations
        /// <summary>
        /// Compares the lower limits.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="xt">The x type.</param>
        /// <param name="y">The y value.</param>
        /// <param name="yt">The y type.</param>
        /// <returns>+1 when x›y and -1 when x‹y, 0 otherwise</returns>
        static int CompareLowerLimits(double x, LimitType xt, double y, LimitType yt)
        {
            // (x-------
            // (y-------
            if (xt == LimitType.Infinite && yt == LimitType.Infinite) return 0;
            // (x-------
            // <---[y---
            if (xt == LimitType.Infinite) return -1;
            // <---[x---
            // (y-------
            if (yt == LimitType.Infinite) return 1;

            // <---[x---
            // <---[y---
            if (x == y && xt == LimitType.Closed && yt == LimitType.Closed) return 0;
            // <---(x---
            // <---[y---
            if (x == y && xt == LimitType.Open && yt == LimitType.Closed) return -1;
            // <---[x---
            // <---(y---
            if (x == y && xt == LimitType.Closed && yt == LimitType.Open) return 1;
            // <---(x---
            // <---(y---
            if (x == y && xt == LimitType.Open && yt == LimitType.Open) return 0;


            return x.CompareTo(y);
        }
        /// <summary>
        /// Compares the upper limits.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="xt">The x type.</param>
        /// <param name="y">The y value.</param>
        /// <param name="yt">The y type.</param>
        /// <returns>+1 when x›y and -1 when x‹y, 0 otherwise</returns>
        static int CompareUpperLimits(double x, LimitType xt, double y, LimitType yt)
        {
            // -------x)
            // -------y)
            if (xt == LimitType.Infinite && yt == LimitType.Infinite) return 0;
            // -------x)
            // ---y]--->
            if (xt == LimitType.Infinite) return 1;

            // ---x]--->
            // -------y)
            if (yt == LimitType.Infinite) return -1;

            // ---x]--->
            // ---y]--->
            if (x == y && xt == LimitType.Closed && yt == LimitType.Closed) return 0;
            // ---x)--->
            // ---y]--->
            if (x == y && xt == LimitType.Open && yt == LimitType.Closed) return -1;
            // ---x]--->
            // ---y)--->
            if (x == y && xt == LimitType.Open && yt == LimitType.Closed) return 1;
            // ---x)--->
            // ---y)--->
            if (x == y && xt == LimitType.Open && yt == LimitType.Open) return 0;

            return x.CompareTo(y);
        }

        public static Interval Union(Interval a, Interval b)
        {
            //double lower = Math.Min(a.LowerValue, b.LowerValue);
            //double upper = Math.Max(a.UpperValue, b.UpperValue);
            //LimitType ll = lower == a.LowerValue ? a.LowerLimit : b.LowerLimit;
            //LimitType ul = upper == a.UpperValue ? a.UpperLimit : b.UpperLimit;

            //if (a.UpperValue >= b.LowerValue || a.LowerValue <= b.UpperValue)
            //{
            //    var c = new Interval(lower, upper, ll, ul);
            //    Debug.WriteLine($"{a} ⋃ {b} = {c}");
            //    return c;
            //}
            //return Invalid;

            double a_LL = a.LowerValue, b_LL = b.LowerValue;
            double a_UL = a.UpperValue, b_UL = b.UpperValue;
            LimitType a_LT = a.LowerLimit, b_LT = b.LowerLimit;
            LimitType a_UT = a.UpperLimit, b_UT = b.UpperLimit;

            double lower, upper;
            LimitType ll, ul;


            // l => +1 when a>b, -1 when a<b
            int l = CompareLowerLimits(a_LL, a_LT, b_LL, b_LT);
            if (l > 0)
            {
                lower = b_LL;
                ll = b_LT;
            }
            else
            {
                lower = a_LL;
                ll = a_LT;
            }


            // u => +1 when a>b, -1 when a<b
            int u = CompareUpperLimits(a_UL, a_UT, b_UL, b_UT);
            if (u < 0)
            {
                upper = b_UL;
                ul = b_UT;
            }
            else
            {
                upper = a_UL;
                ul = a_UT;
            }

            if (lower <= upper)
            {
                var c = new Interval(lower, upper, ll, ul);
                Debug.WriteLine($"{a} ⋂ {b} = {c}");
                return c;
            }
            return Invalid;

        }

        public static Interval Intersect(Interval a, Interval b)
        {
            double a_LL = a.LowerValue, b_LL = b.LowerValue;
            double a_UL = a.UpperValue, b_UL = b.UpperValue;
            LimitType a_LT = a.LowerLimit, b_LT = b.LowerLimit;
            LimitType a_UT = a.UpperLimit, b_UT = b.UpperLimit;

            double lower, upper;
            LimitType ll, ul;


            // l => +1 when a>b, -1 when a<b
            int l = CompareLowerLimits(a_LL, a_LT, b_LL, b_LT);
            if (l < 0)
            {
                lower = b_LL;
                ll = b_LT;
            }
            else
            {
                lower = a_LL;
                ll = a_LT;
            }


            // u => +1 when a>b, -1 when a<b
            int u = CompareUpperLimits(a_UL, a_UT, b_UL, b_UT);
            if (u > 0)
            {
                upper = b_UL;
                ul = b_UT;
            }
            else
            {
                upper = a_UL;
                ul = a_UT;
            }

            if (lower <= upper)
            {
                var c = new Interval(lower, upper, ll, ul);
                Debug.WriteLine($"{a} ⋂ {b} = {c}");
                return c;
            }
            return Invalid;
        }
        #endregion

        #region Equality
        public override bool Equals(object obj)
        {
            return obj is Interval ival && Equals(ival);
        }

        public bool Equals(Interval other)
        {
            return LowerLimit == other.LowerLimit &&
                   UpperLimit == other.UpperLimit &&
                   LowerValue == other.LowerValue &&
                   UpperValue == other.UpperValue;
        }

        public override int GetHashCode()
        {
            var hashCode = -269256374;
            hashCode = hashCode * -1521134295 + LowerLimit.GetHashCode();
            hashCode = hashCode * -1521134295 + UpperLimit.GetHashCode();
            hashCode = hashCode * -1521134295 + LowerValue.GetHashCode();
            hashCode = hashCode * -1521134295 + UpperValue.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Interval interval1, Interval interval2)
        {
            return interval1.Equals(interval2);
        }

        public static bool operator !=(Interval interval1, Interval interval2)
        {
            return !(interval1 == interval2);
        }

        #endregion

        #region Formatting
        public string ToString(string formatting, IFormatProvider provider)
        {
            if (!IsValid)
            {
                return $"(invld)";
            }
            if (IsNothing)
            {
                return $"()";
            }
            char l = LowerLimit == LimitType.Closed ? '[' : '(';
            char u = UpperLimit == LimitType.Closed ? ']' : ')';

            return $"{l}{LowerValue.ToString(formatting, provider)},{UpperValue.ToString(formatting, provider)}{u}";
        }
        public string ToString(string formatting)
            => ToString(formatting, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
        public override string ToString()
            => ToString("g");
        #endregion

    }
}
