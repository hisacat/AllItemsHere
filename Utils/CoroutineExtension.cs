using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace RF5.HisaCat.AllItemsHere.Utils
{
    internal static class CoroutineExtension
    {
        public static UnityEngine.Coroutine StartCoroutine(this MonoBehaviour from, IEnumerator coroutine) => from.StartCoroutine(new Il2CppSystem.Collections.IEnumerator(new Wrapper(coroutine).Pointer));
        //public static void Stop(this MonoBehaviour from, UnityEngine.Coroutine coroutine) => from.StopCoroutine(coroutine);


        [Il2CppImplements(typeof(Il2CppSystem.Collections.IEnumerator))]
        internal class Wrapper : Il2CppSystem.Object
        {
            private readonly IEnumerator enumerator;

            public Wrapper(IntPtr ptr) : base(ptr) { }
            public Wrapper(IEnumerator enumerator) : base(ClassInjector.DerivedConstructorPointer<Wrapper>())
            {
                ClassInjector.DerivedConstructorBody(this);
                this.enumerator = enumerator;
            }

            //public Il2CppSystem.Object Current
            public UnhollowerBaseLib.Il2CppObjectBase Current
            {
                get
                {
                    if (enumerator.Current == null)
                        return null;
                    else
                    {
                        var type = enumerator.Current.GetType();
                        if (typeof(IEnumerator).IsAssignableFrom(type))
                            return new Wrapper((IEnumerator)enumerator.Current);
                        else if (typeof(UnhollowerBaseLib.Il2CppObjectBase).IsAssignableFrom(type))
                            return (UnhollowerBaseLib.Il2CppObjectBase)enumerator.Current;
                        else
                            throw new NotSupportedException($"{enumerator.GetType()}: Unsupported type {enumerator.Current.GetType()}");
                    }
                }
            }

            public bool MoveNext() => enumerator.MoveNext();
            public void Reset() => enumerator.Reset();
        }

    }
}
