namespace GestaoFinanceira_v_1.Controllers
{
    public class Cabecalho
    {
        public event Action OnChange;
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnChange?.Invoke();
                }
            }
        }

        public event Action TitleChanged;
    }
}
