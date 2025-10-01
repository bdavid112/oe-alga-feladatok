using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T>? kov;

        public LancElem(T tart, LancElem<T>? kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }

    public class LancoltVerem<T> : Verem<T>
    {
        private LancElem<T>? fej;
        public bool Ures => fej == null;

        public void Verembe(T ertek)
        {
            var ujElem = new LancElem<T>(ertek, null);
            if (Ures)
            {
                fej = ujElem;
            }
            else
            {
                var p = fej;
                while (p!.kov != null) p = p.kov;
                p.kov = ujElem;
            }
        }

        public T Verembol()
        {
            if (Ures) throw new NincsElemKivetel();

            var p = fej;
            T t;

            while (p?.kov != null)
            {
                if (p.kov.kov == null)
                {
                    t = p.kov.tart;
                    p.kov = null;
                    return t;
                }

                p = p.kov;
            }

            t = p!.tart;
            fej = null;
            return t;
        }

        public T Felso()
        {
            if (Ures) throw new NincsElemKivetel();

            var p = fej;
            while (p?.kov != null) p = p.kov;

            return p!.tart;
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        private LancElem<T>? fej;
        private LancElem<T>? vege;
        public bool Ures => fej == null;

        public void Sorba(T ertek)
        {
            var ujElem = new LancElem<T>(ertek, null);
            if (Ures)
            {
                fej = ujElem;
                vege = ujElem;
            }
            else
            {
                vege!.kov = ujElem;
                vege = vege.kov;
            }
        }

        public T Sorbol()
        {
            if (Ures) throw new NincsElemKivetel();

            var p = fej;
            fej = fej!.kov;

            return p!.tart;
        }

        public T Elso()
        {
            if (Ures) throw new NincsElemKivetel();
            return fej!.tart;
        }
    }

    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        private LancElem<T>? fej;

        public int Elemszam
        {
            get
            {
                var j = fej;
                var db = 0;

                while (j != null)
                {
                    db++;
                    j = j.kov;
                }

                return db;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        public T Kiolvas(int index)
        {
            var elem = ElemLekerdezes(index);
            return elem.tart;
        }

        public void Modosit(int index, T ertek)
        {
            var elem = ElemLekerdezes(index);
            elem.tart = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            var ujElem = new LancElem<T>(ertek, null);
            if (Elemszam <= 0)
            {
                fej = ujElem;
            }
            else
            {
                var p = fej;
                while (p!.kov != null) p = p.kov;
                p.kov = ujElem;
            }
        }

        public void Beszur(int index, T ertek)
        {
            LancElem<T> elem;
            LancElem<T> ujElem;

            /* Utolsó helyre beszúrás nem üres listába */
            if (Elemszam > 0 && index == Elemszam)
            {
                elem = ElemLekerdezes(index - 1);
                ujElem = new LancElem<T>(ertek, null);
                elem.kov = ujElem;
                return;
            }

            elem = ElemLekerdezes(index);

            /* ELső helyre beszúrás */
            if (index == 0)
            {
                ujElem = new LancElem<T>(ertek, elem);
                fej = ujElem;
                return;
            }

            /* Általános eset */
            ujElem = new LancElem<T>(elem.tart, elem.kov);
            elem.tart = ertek;
            elem.kov = ujElem;
        }

        public void Torol(T ertek)
        {
            var p = fej;

            while (p != null)
            {
                var q = p;

                while (q != null && q.tart != null && q.tart.Equals(ertek))
                {
                    q = q.kov;
                }

                if (q != null)
                {
                    p.tart = q.tart;
                    p.kov = q.kov;
                }

                p = p.kov;
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            foreach (var elem in this)
            {
                muvelet.Invoke(elem);
            }
        }

        private LancElem<T> ElemLekerdezes(int index)
        {
            if (index < 0) throw new HibasIndexKivetel();

            var p = fej;
            var elemszam = Elemszam;

            for (var i = 0; i < elemszam && i < index; i++)
            {
                p = p!.kov;
            }

            return p!;
        }
    }

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        private LancElem<T>? fej;
        private LancElem<T>? aktualisElem;
        object? IEnumerator.Current => Current;
        public T Current => aktualisElem != null ? aktualisElem.tart : throw new NincsElemKivetel();

        public LancoltListaBejaro(LancElem<T>? fej)
        {
            this.fej = fej;
            aktualisElem = null;
        }

        public bool MoveNext()
        {
            aktualisElem = aktualisElem == null ? fej : aktualisElem.kov;
            return aktualisElem != null;
        }

        public void Reset()
        {
            aktualisElem = null;
        }

        public void Dispose()
        {
        }
    }
}