//the test case from java version
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
public class SolutionTest
{

    private static string[] msg =
    {
        "should support finding peaks",
        "should support finding peaks, but should ignore peaks on the edge of the array",
        "should support finding peaks; if the peak is a plateau, it should only return the position of the first element of the plateau",
        "should support finding peaks; if the peak is a plateau, it should only return the position of the first element of the plateau",
        "should support finding peaks, but should ignore peaks on the edge of the array",
        "should support finding peaks, but should ignore peaks on the edge of the array",
        "should support finding peaks, despite the plateau",
        "should support finding peaks",
        "should return an object with empty arrays if the input is an empty array",
        "should return an object with empty arrays if the input does not contain any peak"
    };

    private static int[][] array =
    {
        new int []{1,2,3,6,4,1,2,3,2,1},
        new int []{3,2,3,6,4,1,2,3,2,1,2,3},
        new int []{3,2,3,6,4,1,2,3,2,1,2,2,2,1},
        new int []{2,1,3,1,2,2,2,2,1},
        new int []{2,1,3,1,2,2,2,2},
        new int []{2,1,3,2,2,2,2,5,6},
        new int []{2,1,3,2,2,2,2,1},
        new int []{1,2,5,4,3,2,3,6,4,1,2,3,3,4,5,3,2,1,2,3,5,5,4,3},
        new int []{},
        new int []{1,1,1,1}
    };

    private static int[][] posS =
    {
        new int []{3,7},
        new int []{3,7},
        new int []{3,7,10},
        new int []{2,4},
        new int []{2},
        new int []{2},
        new int []{2},
        new int []{2,7,14,20},
        new int []{},
        new int []{}
    };

    private static int[][] peaksS =
    {
        new int []{6,3},
        new int []{6,3},
        new int []{6,3,2},
        new int []{3,2},
        new int []{3},
        new int []{3},
        new int []{3},
        new int []{5,6,5,5},
        new int []{},
        new int []{}
    };

    [Test]
    public void SampleTests()
    {
        for (int n = 0; n < 5; n++)
        {
            int[] p1 = posS[n], p2 = peaksS[n];
            var expected = new Dictionary<string, List<int>>()
            {
                ["pos"] = p1.ToList(),
                ["peaks"] = p2.ToList()
            };
            var actual = PickPeaks.GetPeaks(array[n]);
            Assert.AreEqual(expected, actual, msg[n]);
        }
    }


    [Test]
    public void MoreTests()
    {
        for (int n = 5; n < msg.Length; n++)
        {
            int[] p1 = posS[n], p2 = peaksS[n];
            var expected = new Dictionary<string, List<int>>()
            {
                ["pos"] = p1.ToList(),
                ["peaks"] = p2.ToList()
            };
            var actual = PickPeaks.GetPeaks(array[n]);
            Assert.AreEqual(expected, actual, msg[n]);
        }
    }


    private Random rand = new Random();


    [Test]
    public void RandomTests()
    {
        for (int n = 0; n < 40; n++)
        {

            int[] arr = new int[5 + rand.Next(25)];
            for (int i = 0; i < arr.Length; i++) arr[i] = -5 + rand.Next(25);

            var expected = GetPeaks(arr);
            var actual = PickPeaks.GetPeaks(arr);
            Assert.AreEqual(expected, actual, $"Testing for {string.Join(", ", arr)}:");
        }
    }


    private static Dictionary<string, List<int>> GetPeaks(int[] arr)
    {
        var ans = new Dictionary<string, List<int>>()
        {
            ["pos"] = new List<int>(),
            ["peaks"] = new List<int>()
        };
        var posMax = 0;
        var matchAsc = false;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i - 1] < arr[i])
            {
                matchAsc = true;
                posMax = i;
            }
            if (matchAsc && arr[i - 1] > arr[i])
            {
                matchAsc = false;
                ans["pos"].Add(posMax);
                ans["peaks"].Add(arr[posMax]);
            }
        }
        return ans;
    }
}