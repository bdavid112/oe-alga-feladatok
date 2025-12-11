using System;
using System.Linq;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl(int honnan, int hova, float suly)
        : EgeszGrafEl(honnan, hova), SulyozottGrafEl<int>
    {
        public float Suly { get; } = suly;
    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        private int _n;
        private float[,] _m;

        public int CsucsokSzama => _n;
        public int ElekSzama => _m.Cast<float>().Count(float.IsNaN);

        public Halmaz<int> Csucsok
        {
            get
            {
                var halmaz = new FaHalmaz<int>();
                for (var i = 0; i < _n; i++)
                {
                    halmaz.Beszur(i);
                }

                return halmaz;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                var halmaz = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (var i = 0; i < _n; i++)
                {
                    for (var j = 0; j < _n; j++)
                    {
                        if (float.IsNaN(_m[i, j])) continue;

                        var el = new SulyozottEgeszGrafEl(i, j, Suly(i, j));
                        halmaz.Beszur(el);
                    }
                }

                return halmaz;
            }
        }

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            _n = n;
            _m = new float[n, n];

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    _m[i, j] = float.NaN;
                }
            }
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(_m[honnan, hova]);
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            var halmaz = new FaHalmaz<int>();
            Csucsok.Bejar(cs =>
            {
                if (VezetEl(csucs, cs)) halmaz.Beszur(cs);
            });
            return halmaz;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            _m[honnan, hova] = suly;
        }

        public float Suly(int honnan, int hova)
        {
            return VezetEl(honnan, hova) ? _m[honnan, hova] : throw new NincsElKivetel();
        }
    }

    public static class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start)
        {
            var l = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            var p = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);

            var s = new KupacPrioritasosSor<V>(
                g.CsucsokSzama,
                (a, b) => l.Kiolvas(a) < l.Kiolvas(b)
            );

            g.Csucsok.Bejar(x =>
            {
                l.Beir(x, float.PositiveInfinity);
                s.Sorba(x);
            });

            l.Beir(start, 0);
            s.Frissit(start);

            while (!s.Ures)
            {
                var u = s.Sorbol();
                g.Szomszedai(u).Bejar(x =>
                {
                    var uj = l.Kiolvas(u) + g.Suly(u, x);
                    if (!(l.Kiolvas(x) > uj)) return;

                    l.Beir(x, uj);
                    p.Beir(x, u);
                    s.Frissit(x);
                });
            }

            return l;
        }
    }

    public static class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable<V>
        {
            var k = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            var p = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);

            var s = new KupacPrioritasosSor<V>(
                g.CsucsokSzama,
                (a, b) => k.Kiolvas(a) < k.Kiolvas(b)
            );

            var h = new FaHalmaz<V>();

            g.Csucsok.Bejar(x =>
            {
                k.Beir(x, float.PositiveInfinity);
                s.Sorba(x);
                h.Beszur(x);
            });

            k.Beir(start, 0);
            s.Frissit(start);

            while (!s.Ures)
            {
                var u = s.Sorbol();
                h.Torol(u);

                g.Szomszedai(u).Bejar(x =>
                {
                    var suly = g.Suly(u, x);
                    if (!h.Eleme(x) || !(suly < k.Kiolvas(x))) return;

                    k.Beir(x, suly);
                    s.Frissit(x);
                    p.Beir(x, u);
                });
            }

            return p;
        }

        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g)
            where E : SulyozottGrafEl<V>, IComparable<E>
            where V : IComparable<V>
        {
            var halmazok = new HasitoSzotarTulcsordulasiTerulettel<V, int>(g.CsucsokSzama);
            var s = new KupacPrioritasosSor<E>(g.ElekSzama * 2, (a, b) => a.Suly < b.Suly);
            var a = new FaHalmaz<E>();

            var i = 0;

            g.Csucsok.Bejar(v => { halmazok.Beir(v, i++); });

            g.Elek.Bejar(e => s.Sorba(e));

            while (!s.Ures)
            {
                var e = s.Sorbol();

                var compA = halmazok.Kiolvas(e.Honnan);
                var compB = halmazok.Kiolvas(e.Hova);

                if (compA == compB) continue;

                a.Beszur(e);

                g.Csucsok.Bejar(v =>
                {
                    if (halmazok.Kiolvas(v) == compB)
                        halmazok.Beir(v, compA);
                });
            }

            return a;
        }
    }
}