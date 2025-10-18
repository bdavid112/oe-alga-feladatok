using System;

namespace OE.ALGA.Adatszerkezetek
{
    internal class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        private SzotarElem<K, T>?[] E;
        private Func<K, int> h;
        private LancoltLista<SzotarElem<K, T>> U = new();

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            h = kulcs => Math.Abs(hasitoFuggveny(kulcs)) % E.Length;
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret)
        {
            E = new SzotarElem<K, T>[meret];
            h = kulcs => Math.Abs(kulcs!.GetHashCode()) % E.Length;
        }

        private SzotarElem<K, T>? KulcsKeres(K kulcs)
        {
            var k = h(kulcs);
            if (E[k] != null && E[k]!.kulcs!.Equals(kulcs)) return E[k];

            SzotarElem<K, T>? elem = null;
            U.Bejar(x =>
            {
                if (x.kulcs != null && x.kulcs.Equals(kulcs)) elem = x;
            });

            return elem;
        }

        public void Beir(K kulcs, T ertek)
        {
            var letezoElem = KulcsKeres(kulcs);

            if (letezoElem != null)
            {
                letezoElem.tart = ertek;
                return;
            }

            var uj = new SzotarElem<K, T>(kulcs, ertek);
            var k = h(kulcs);

            if (E[k] == null) E[k] = uj;
            else
            {
                U.Hozzafuz(uj);
            }
        }

        public T Kiolvas(K kulcs)
        {
            var letezoElem = KulcsKeres(kulcs);
            if (letezoElem == null) throw new HibasKulcsKivetel();
            return letezoElem.tart;
        }

        public void Torol(K kulcs)
        {
            var k = h(kulcs);
            if (E[k] != null && E[k]!.kulcs!.Equals(kulcs))
            {
                E[k] = null;
                return;
            }

            SzotarElem<K, T>? elem = null;
            U.Bejar(x =>
            {
                if (x.kulcs != null && x.kulcs.Equals(kulcs)) elem = x;
            });

            if (elem == null) throw new HibasKulcsKivetel();
            U.Torol(elem);
        }
    }
}