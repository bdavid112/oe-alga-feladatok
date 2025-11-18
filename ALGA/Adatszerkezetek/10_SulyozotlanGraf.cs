using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl(int honnan, int hova) : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public int Honnan { get; } = honnan;
        public int Hova { get; } = hova;

        public int CompareTo(EgeszGrafEl? other)
        {
            if (other is null) throw new ArgumentException("Null argument");
            return Honnan != other.Honnan ? Honnan.CompareTo(other.Honnan) : Hova.CompareTo(other.Hova);
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf(int n) : SulyozatlanGraf<int, EgeszGrafEl>
    {
        private int n = n;
        private bool[,] m = new bool[n, n];
        public int CsucsokSzama { get; } = n;

        public int ElekSzama
        {
            get
            {
                var sum = 0;
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        if (m[i, j]) sum++;
                    }
                }

                return sum;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                var csucsok = new FaHalmaz<int>();
                for (var i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }

                return csucsok;
            }
        }

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                var elek = new FaHalmaz<EgeszGrafEl>();
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        if (m[i, j]) elek.Beszur(new EgeszGrafEl(i, j));
                    }
                }

                return elek;
            }
        }

        public bool VezetEl(int honnan, int hova)
        {
            return m[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            var szomszedai = new FaHalmaz<int>();

            for (var i = 0; i < n; i++)
            {
                if (m[csucs, i]) szomszedai.Beszur(i);
            }

            return szomszedai;
        }

        public void UjEl(int honnan, int hova)
        {
            m[honnan, hova] = true;
        }
    }

    public static class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet)
            where V : IComparable<V>
        {
            var s = new LancoltSor<V>();
            var f = new FaHalmaz<V>();
            s.Sorba(start);
            f.Beszur(start);

            while (!s.Ures)
            {
                var k = s.Sorbol();
                muvelet(k);

                g.Szomszedai(k).Bejar(x =>
                {
                    if (f.Eleme(x)) return;

                    s.Sorba(x);
                    f.Beszur(x);
                });
            }

            return f;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            var f = new FaHalmaz<V>();
            f.Beszur(start);
            MelysegiBejarasRekurzio(g, start, f, muvelet);
            return f;
        }

        private static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> f, Action<V> muvelet)
            where V : IComparable<V>
        {
            f.Beszur(k);
            muvelet(k);

            var halmaz = f;
            g.Szomszedai(k).Bejar(x =>
            {
                if (halmaz.Eleme(x)) return;
                MelysegiBejarasRekurzio(g, x, f, muvelet);
            });
        }
    }
}