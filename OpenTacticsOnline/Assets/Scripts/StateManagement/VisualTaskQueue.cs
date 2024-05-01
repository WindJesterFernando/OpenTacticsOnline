using System.Collections.Generic;

public static class VisualTaskQueue
{
    private static Queue<VisualTask> taskQueue;
    
    public static void Init()
    {
        taskQueue = new Queue<VisualTask>();
    }

    public static void Reset()
    {
        taskQueue.Clear();
    }

    public static void Update()
    {
        if (taskQueue.Count > 0)
        {
            taskQueue.Peek().Update();
            if(taskQueue.Peek().IsDone)
                taskQueue.Dequeue();
        }
    }

    public static void EnqueueAction(VisualTask action)
    {
        taskQueue.Enqueue(action);
    }

    public static int GetActionCount()
    {
        return taskQueue.Count;
    }
}
