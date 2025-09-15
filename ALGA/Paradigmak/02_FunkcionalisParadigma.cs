using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Predicate<T>? BejaroFeltetel { get; set; }

        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {
        }

        public void FeltetelesVegrehajtas(Predicate<T> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (!feltetel(tarolo[i])) continue;
                tarolo[i].Vegrehajtas();
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel ?? (_ => true));
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        private T[] tarolo;
        private int n;
        private int aktualisIndex = -1;
        private Predicate<T> bejaroFeltetel;

        public T Current
        {
            get
            {
                if (aktualisIndex < 0 || aktualisIndex >= n)
                    throw new InvalidOperationException();
                return tarolo[aktualisIndex];
            }
        }

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Predicate<T> bejaroFeltetel)
        {
            this.tarolo = tarolo;
            this.n = n;
            this.bejaroFeltetel = bejaroFeltetel;
        }

        public bool MoveNext()
        {
            while (++aktualisIndex < n)
            {
                if (bejaroFeltetel(tarolo[aktualisIndex])) return true;
            }

            return false;
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}