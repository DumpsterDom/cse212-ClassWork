using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue items with different priorities and dequeue them
    // Expected Result: Items are dequeued in descending order of priority (highest first)
    // Defect(s) Found: 
    // 1. Loop only goes to Count-2 misses checking the last item
    // 2. Dequeue returns value but does not remove it from the list
    public void TestPriorityQueue_HighestPriorityFirst()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("Low", 1);
        pq.Enqueue("Medium", 5);
        pq.Enqueue("High", 10);

        Assert.AreEqual("High", pq.Dequeue());
        Assert.AreEqual("Medium", pq.Dequeue());
        Assert.AreEqual("Low", pq.Dequeue());

        Assert.AreEqual(0, pq.ToString().Count(c => c == ','), "Queue should be empty after all dequeues");
    }

    [TestMethod]
    // Scenario: Multiple items with the same highest priority
    // Expected Result: FIFO order — the first-enqueued item with highest priority is dequeued first
    // Defect(s) Found: 
    // 1. Uses >= instead of > when priorities are equal, it picks the last one not the first.
    public void TestPriorityQueue_SamePriority_FIFO()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("First High", 10);
        pq.Enqueue("Low", 2);
        pq.Enqueue("Second High", 10);
        pq.Enqueue("Medium", 5);

        Assert.AreEqual("First High", pq.Dequeue()); 
        Assert.AreEqual("Second High", pq.Dequeue());
        Assert.AreEqual("Medium", pq.Dequeue());
        Assert.AreEqual("Low", pq.Dequeue());
    }

    [TestMethod]
    // Scenario: Dequeue from an empty queue
    // Expected Result: Throws InvalidOperationException with correct message
    // Defect(s) Found: None 
    public void TestPriorityQueue_EmptyDequeue()
    {
        var pq = new PriorityQueue();

        try
        {
            pq.Dequeue();
            Assert.Fail("Expected InvalidOperationException");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("The queue is empty.", ex.Message);
        }
    }

    [TestMethod]
    // Scenario: Enqueue after some dequeues + mixed priorities
    // Expected Result: Newly added high-priority item is dequeued next if highest
    // Defect(s) Found: missing remove & wrong index selection
    public void TestPriorityQueue_EnqueueAfterDequeue()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 3);
        pq.Enqueue("B", 1);

        Assert.AreEqual("A", pq.Dequeue());

        pq.Enqueue("C", 5);
        pq.Enqueue("D", 4);

        Assert.AreEqual("C", pq.Dequeue());
        Assert.AreEqual("D", pq.Dequeue());
        Assert.AreEqual("B", pq.Dequeue());
    }
}