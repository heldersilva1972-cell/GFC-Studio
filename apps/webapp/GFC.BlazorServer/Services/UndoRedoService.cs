// [NEW]
namespace GFC.BlazorServer.Services
{
    public class UndoRedoService<T> where T : class
    {
        private readonly List<T> _history = new();
        private int _currentIndex = -1;

        public bool CanUndo => _currentIndex > 0;
        public bool CanRedo => _currentIndex < _history.Count - 1;

        public void RecordState(T state)
        {
            // If we are in the middle of the history, remove the future states
            if (_currentIndex < _history.Count - 1)
            {
                _history.RemoveRange(_currentIndex + 1, _history.Count - _currentIndex - 1);
            }

            _history.Add(Clone(state));
            _currentIndex++;
        }

        public T Undo()
        {
            if (CanUndo)
            {
                _currentIndex--;
                return Clone(_history[_currentIndex]);
            }
            return null;
        }

        public T Redo()
        {
            if (CanRedo)
            {
                _currentIndex++;
                return Clone(_history[_currentIndex]);
            }
            return null;
        }

        private T Clone(T source)
        {
            // This is a simple deep clone using JSON serialization.
            // For complex types, a more robust cloning strategy might be needed.
            var json = System.Text.Json.JsonSerializer.Serialize(source);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }
}
