using System;

namespace OE.ALGA.Adatszerkezetek
{
    internal class FaElem<T> where T : IComparable<T>
    {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;

        public FaElem(T tart, FaElem<T>? bal, FaElem<T>? jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        private FaElem<T>? gyoker = null;

        public void Beszur(T ertek)
        {
            if (gyoker != null) ReszfabaBeszur(gyoker, ertek);
            else gyoker = new FaElem<T>(ertek, null, null);
        }

        private static void ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            var cmp = p.tart.CompareTo(ertek);

            switch (cmp)
            {
                case > 0:
                    if (p.bal != null) ReszfabaBeszur(p.bal, ertek);
                    else p.bal = new FaElem<T>(ertek, null, null);

                    break;
                case < 0:
                    if (p.jobb != null) ReszfabaBeszur(p.jobb, ertek);
                    else p.jobb = new FaElem<T>(ertek, null, null);

                    break;
            }
        }

        public bool Eleme(T ertek)
        {
            return gyoker != null && ReszfaEleme(gyoker, ertek);
        }

        private static bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            var cmp = p.tart.CompareTo(ertek);
            return cmp switch
            {
                > 0 when p.bal != null => ReszfaEleme(p.bal, ertek),
                < 0 when p.jobb != null => ReszfaEleme(p.jobb, ertek),
                0 => true,
                _ => false
            };
        }

        public void Torol(T ertek)
        {
            if (gyoker != null) gyoker = ReszfabolTorol(gyoker, ertek);
        }

        private static FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p == null) throw new NincsElemKivetel();
            var cmp = p.tart.CompareTo(ertek);

            switch (cmp)
            {
                case > 0:
                    p.bal = ReszfabolTorol(p.bal, ertek);
                    break;
                case < 0:
                    p.jobb = ReszfabolTorol(p.jobb, ertek);
                    break;
                case 0:
                    if (p.bal == null) p = p.jobb;
                    else if (p.jobb == null) p = p.bal;
                    else p.bal = KetGyerekesTorles(p.bal, p);
                    break;
            }

            return p;
        }

        private static FaElem<T> KetGyerekesTorles(FaElem<T> r, FaElem<T> e)
        {
            if (r.jobb != null) r.jobb = KetGyerekesTorles(r.jobb, e);
            else
            {
                e.tart = r.tart;
                r = r.bal;
            }

            return r;
        }

        public void Bejar(Action<T> muvelet)
        {
            if (gyoker != null) ReszfaBejarasPreOrder(gyoker, muvelet);
        }

        private static void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            muvelet(p.tart);
            if (p.bal != null) ReszfaBejarasPreOrder(p.bal, muvelet);
            if (p.jobb != null) ReszfaBejarasPreOrder(p.jobb, muvelet);
        }

        private static void ReszfaBejarasInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p.bal != null) ReszfaBejarasInOrder(p.bal, muvelet);
            muvelet(p.tart);
            if (p.jobb != null) ReszfaBejarasInOrder(p.jobb, muvelet);
        }

        private static void ReszfaBejarasPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p.bal != null) ReszfaBejarasPostOrder(p.bal, muvelet);
            if (p.jobb != null) ReszfaBejarasPostOrder(p.jobb, muvelet);
            muvelet(p.tart);
        }
    }
}