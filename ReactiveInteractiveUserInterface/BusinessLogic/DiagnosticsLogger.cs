using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic;

/// <summary>
/// Thread-safe, non-blocking ASCII logger.  Drop logs and keep the simulation
/// smooth if the disk can’t keep up.
/// </summary>
internal sealed class DiagnosticsLogger : IDisposable
{
    private readonly BlockingCollection<string> _queue;
    private readonly Task _writerTask;
    private readonly CancellationTokenSource _cts = new();

    public DiagnosticsLogger(string filePath,
                             int maxQueue = 10_000)
    {
        _queue = new BlockingCollection<string>(maxQueue);

        _writerTask = Task.Run(async () =>
        {
            await using var stream = new FileStream(
                    filePath, FileMode.Append, FileAccess.Write,
                    FileShare.Read, bufferSize: 4096, useAsync: true);
            await using var writer = new StreamWriter(stream, Encoding.ASCII);

            foreach (var line in _queue.GetConsumingEnumerable(_cts.Token))
            {
                await writer.WriteLineAsync(line);
            }
        }, _cts.Token);
    }

    public void Log(int id, IVector pos, IVector vel)
    {
        var stamp = DateTime.UtcNow.ToString("O");   // ISO-8601
        var line = $"{stamp};{id};{pos.x:F2};{pos.y:F2};{vel.x:F2};{vel.y:F2}";

        // If the queue is full, drop the line – keep real-time guarantees.
        _queue.TryAdd(line);
    }

    public void Dispose()
    {
        _queue.CompleteAdding();
        _cts.Cancel();
        //_writerTask.Wait();
    }
}