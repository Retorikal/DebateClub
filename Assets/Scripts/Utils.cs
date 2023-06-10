using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumUtils {
  public struct Pair<T, U> {
    public T first;
    public U second;

    public Pair(T t, U u) {
      first = t;
      second = u;
    }
  }

  public static IEnumerable<Pair<T, U>> Zip<T, U>(IEnumerable<T> first, IEnumerable<U> second) {
    IEnumerator<T> firstEnumerator = first.GetEnumerator();
    IEnumerator<U> secondEnumerator = second.GetEnumerator();

    while (firstEnumerator.MoveNext()) {
      if (secondEnumerator.MoveNext()) {
        yield return new Pair<T, U>(firstEnumerator.Current, secondEnumerator.Current);
      } else {
        yield return new Pair<T, U>(firstEnumerator.Current, default);
      }
    }

    while (secondEnumerator.MoveNext()) {
      yield return new Pair<T, U>(default, secondEnumerator.Current);
    }
  }

  public static void Shuffle<T>(IList<T> list) {
    int n = list.Count;
    while (n > 1) {
      n--;
      int k = UnityEngine.Random.Range(0, n + 1);
      T value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
  }

  public static IList<T> Shuffled<T>(IList<T> list) {
    var newList = new List<T>(list);
    int n = newList.Count;
    while (n > 1) {
      n--;
      int k = UnityEngine.Random.Range(0, n + 1);
      T value = newList[k];
      newList[k] = newList[n];
      newList[n] = value;
    }
    return newList;
  }
}