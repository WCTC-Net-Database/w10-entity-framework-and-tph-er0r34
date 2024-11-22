using W9_assignment_template.Services;

namespace W9_assignment_template
{
    public class GameEngine
    {
        private readonly Menu _menu;

        public GameEngine(Menu menu)
        {
            _menu = menu;
        }

        public void Run()
        {
            _menu.Show();
        }
    }
}
