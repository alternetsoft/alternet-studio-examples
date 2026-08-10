def BubbleSort(ct):
  temp = 0
  MyStep = 3
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
  i = bubbleArray.Count - 1
  while i >= 0:
    j = 0
    while j < bubbleArray.Count - 1:
      if ct.IsCancellationRequested:
        ct.ThrowIfCancellationRequested()

      if bubbleArray[j] > bubbleArray[j + 1]:
        YPos1 = MyStep * (j + 1)
        YPos2 = MyStep * (j + 2)
        len1 = bubbleArray[j]
        len2 = bubbleArray[j + 1]
        temp = bubbleArray[j]
        bubbleArray[j] = bubbleArray[j + 1]
        bubbleArray[j + 1] = temp
        ProgressManager.Refresh("bubble")
      j = j + 1 
    i = i - 1

def SelectionSort(ct):
  temp = 0
  MyStep = 3
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
  i = selArray.Count - 1
  while  i >=0:
    j = selArray.Count - 1
    while j >=0:
      if selArray[i] > selArray[j]:
        if ct.IsCancellationRequested:
          ct.ThrowIfCancellationRequested()

        YPos1 = MyStep * (i + 1)
        YPos2 = MyStep * (j + 1)
        len1 = selArray[i]
        len2 = selArray[j]
        temp = selArray[i]
        selArray[i] = selArray[j]
        selArray[j] = temp
        ProgressManager.Refresh("select");
      j = j - 1
    i = i - 1

def DoQuickSort(iLo, iHi, ct):
  temp = 0
  MyStep = 3
  lo = iLo
  hi = iHi
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0

  midpoint = (lo + hi) // 2

  mid = quickArray[midpoint]
  while lo <= hi:
    if ct.IsCancellationRequested:
        ct.ThrowIfCancellationRequested()

    while quickArray[lo] < mid:
      lo = lo + 1

    while quickArray[hi] > mid:
      hi = hi - 1

    if lo <= hi:
      YPos1 = MyStep * (lo + 1)
      YPos2 = MyStep * (hi + 1)
      len1 = quickArray[lo]
      len2 = quickArray[hi]
      temp = quickArray[lo]
      quickArray[lo] = quickArray[hi]
      quickArray[hi] = temp
      lo = lo + 1
      hi = hi - 1
      ProgressManager.Refresh("quick");

  if hi > iLo:
      DoQuickSort(iLo, hi, ct)
  if lo < iHi:
      DoQuickSort(lo, iHi, ct)


def QuickSort(ct):
  DoQuickSort(0, quickArray.Count - 1, ct)
