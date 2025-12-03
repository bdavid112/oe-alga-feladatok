using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] e;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritas;

        public Kupac(T[] e, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.e = e;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }
        
        public static int Bal(int i) => 2 * (i + 1) - 1;
        
        public static int Jobb(int i) => 2 * (i + 1);

        public static int Szulo(int i) => Math.Max(0, (i + 1) / 2 - 1);

        protected void Kupacol(int i)
        {
            var b = Bal(i);
            var j = Jobb(i);

            var max = i;
            
            if (b < n && nagyobbPrioritas(e[b], e[max])) max = b;
            if (j < n && nagyobbPrioritas(e[j], e[max])) max = j;

            if (max == i) return;

            (e[i], e[max]) = (e[max], e[i]);
            Kupacol(max);
        }

        protected void KupacotEpit()
        {
            for (var i = n / 2; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T>(T[] e) : Kupac<T>(e, e.Length, (a, b) => a.CompareTo(b) == 1)
        where T : IComparable<T>
    {
        public void Rendezes()
        {
            KupacotEpit();
            for (var i = n - 1; i >= 1; i--)
            {
               (e[i], e[0]) = (e[0], e[i]);
               n--;
               Kupacol(0);
            }
        }
    }

    public class KupacPrioritasosSor<T>(int meret, Func<T, T, bool> nagyobbPrioritas)
        : Kupac<T>(new T[meret], 0, nagyobbPrioritas), PrioritasosSor<T>
    {
        public bool Ures => n == 0;
        
        public void Sorba(T ertek)
        {
            if (n >= meret) throw new NincsHelyKivetel();
            e[n++] =  ertek;
            KulcsotFelvisz(n - 1);
        }

        public T Sorbol()
        {
            var elso = Elso();
            (e[0], e[n - 1]) = (e[n - 1], e[0]);
            n--;
            Kupacol(0);
            return elso;
        }

        public T Elso()
        {
            return Ures ? throw new NincsElemKivetel() : e[0];
        }

        public void Frissit(T elem)
        {
            var index = -1;

            for (var i = 0; i < n; i++)
            {
                if (e[i]!.Equals(elem)) index = i;
            }
            
            if (index == -1) throw new NincsElemKivetel();
            
            KulcsotFelvisz(index);
            Kupacol(index);
        }

        private void KulcsotFelvisz(int i)
        {
            var sz = Szulo(i);
            if (!nagyobbPrioritas(e[i], e[sz])) return;
            
            (e[sz],  e[i]) = (e[i], e[sz]);
            KulcsotFelvisz(sz);
        }
    }
}