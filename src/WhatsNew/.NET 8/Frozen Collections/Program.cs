using BenchmarkDotNet.Running;

using Frozen_Collections;

//{
//    var bm = new Set_Construction();
//    bm.Size = 10;
//    bm.GlobalSetup();
//    bm.HashSet_Construction();
//}

_ = BenchmarkRunner.Run<Sets_Construction>();

//_ = BenchmarkRunner.Run<Sets_Access>();

//{
//    var bm = new Dictionaries_Construction();
//    bm.Size = 1_000_000;
//    bm.GlobalSetup();

//    Console.WriteLine("Dictionary_Construction()");
//    bm.Dictionary_Construction();
//    Console.WriteLine("END Dictionary_Construction()");

//    Console.WriteLine("ImmutableDictionary_Construction()");
//    bm.ImmutableDictionary_Construction();
//    Console.WriteLine("END ImmutableDictionary_Construction()");

//    Console.WriteLine("FrozenDictionary_Construction()");
//    bm.FrozenDictionary_Construction();
//    Console.WriteLine("END FrozenDictionary_Construction()");

//    Console.WriteLine("SortedDictionary_Construction()");
//    bm.SortedDictionary_Construction();
//    Console.WriteLine("END SortedDictionary_Construction()");

//    Console.WriteLine("ImmutableSortedDictionary_Construction()");
//    bm.ImmutableSortedDictionary_Construction();
//    Console.WriteLine("END ImmutableSortedDictionary_Construction()");

//    Console.WriteLine("SortedList_Construction()");
//    bm.SortedList_Construction();
//    Console.WriteLine("END SortedList_Construction()");

//    Console.WriteLine("Sorted_Array_Construction()");
//    bm.Sorted_Array_Construction();
//    Console.WriteLine("END Sorted_Array_Construction()");

//    Console.WriteLine("Sorted_ImmutableArray_Construction()");
//    bm.Sorted_ImmutableArray_Construction();
//    Console.WriteLine("END Sorted_ImmutableArray_Construction()");
//}

//_ = BenchmarkRunner.Run<Dictionaries_Construction>();

//_ = BenchmarkRunner.Run<Dictionaries_Access>();
