using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> : Verem<T>
    {
        private T[] E { get; }
        private int n = -1;

        public bool Ures => n < 0;

        public TombVerem(int meret)
        {
            E = new T[meret];
        }

        public T Felso()
        {
            if (Ures) throw new NincsElemKivetel();
            return E[n];
        }

        public void Verembe(T ertek)
        {
            if (n >= E.Length - 1) throw new NincsHelyKivetel();
            E[++n] = ertek;
        }

        public T Verembol()
        {
            if (Ures) throw new NincsElemKivetel();
            return E[n--];
        }
    }

    public class TombSor<T> : Sor<T>
    {
        private T[] E { get; }
        private int e;
        private int u;
        private int n = -1;

        public bool Ures => n < 0;

        public TombSor(int meret)
        {
            E = new T[meret];
        }

        public T Elso()
        {
            if (Ures) throw new NincsElemKivetel();
            return E[e];
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length - 1) throw new NincsHelyKivetel();
            u %= E.Length;
            E[u++] = ertek;
            n++;
        }

        public T Sorbol()
        {
            if (Ures) throw new NincsElemKivetel();
            n--;
            e %= E.Length;
            return E[e++];
        }
    }

    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        private T[] E { get; set; }
        private int n = -1;

        public TombLista(int meret = 1)
        {
            E = new T[meret];
        }

        public int Elemszam => n + 1;

        public void Bejar(Action<T> muvelet)
        {
            for (var i = 0; i < Elemszam; i++)
            {
                muvelet.Invoke(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > n + 1) throw new HibasIndexKivetel();

            if (++n >= E.Length)
            {
                MeretNovelese();
            }

            for (var i = n; i > index; i--)
            {
                E[i] = E[i - 1];
            }

            E[index] = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(n + 1, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index < 0 || index > n) throw new HibasIndexKivetel();
            return E[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index < 0 || index > n) throw new HibasIndexKivetel();
            E[index] = ertek;
        }

        public void Torol(T ertek)
        {
            var db = 0;

            for (var i = 0; i <= n; i++)
            {
                if (E[i]!.Equals(ertek))
                {
                    db++;
                }
                else
                {
                    E[i - db] = E[i];
                }
            }

            n -= db;
        }

        private void MeretNovelese()
        {
            T[] ujTomb = new T[E.Length * 2];
            for (var i = 0; i < E.Length; i++)
            {
                ujTomb[i] = E[i];
            }

            E = ujTomb;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        private T[] E { get; }
        private int n;
        private int aktualisIndex = -1;

        public T Current
        {
            get
            {
                if (aktualisIndex < 0 || aktualisIndex > n)
                    throw new InvalidOperationException();
                return E[aktualisIndex];
            }
        }

        object IEnumerator.Current => Current;

        public TombListaBejaro(T[] E, int n)
        {
            this.E = E;
            this.n = n;
        }

        public bool MoveNext()
        {
            aktualisIndex++;
            return aktualisIndex <= n;
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }

        public void Dispose()
        {
        }
    }
}