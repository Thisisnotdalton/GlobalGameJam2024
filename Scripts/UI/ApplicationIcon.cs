using System;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class ApplicationIcon : Button, IApplicationIcon
    {
        [Export] private NodePath _labelPath = new NodePath(".");
        private Label _label;
        [Export] private NodePath _iconPath = new NodePath(".");
        private TextureRect _icon;
        [Export] private NodePath _selectedEffectPath = new NodePath(".");
        private Control _selectedEffect;
        private TaskBar _taskBar;
        private WindowContainer _windowContainer;
        private TaskBarItem _taskBarItem;
        private ApplicationIconContainer _applicationIconContainer;

        public override void _Ready()
        {
            base._Ready();
            _label = GetNode<Label>(_labelPath);
            if (_label == null)
            {
                throw new Exception(
                    $"Failed to find a {nameof(Label)} for {nameof(ApplicationIcon)} {Name} with path {_labelPath}!");
            }

            _icon = GetNode<TextureRect>(_iconPath);
            if (_label == null)
            {
                throw new Exception(
                    $"Failed to find a {nameof(TextureRect)} for {nameof(ApplicationIcon)} {Name} with path {_iconPath}!");
            }

            if (GetNode(_selectedEffectPath) is Control controlNode)
            {
                _selectedEffect = controlNode;
            }
        }


        private void Initialize(WindowContainer windowContainer, TaskBar taskBar, ApplicationIconContainer applicationIconContainer, ProgramResource applicationData)
        {
            _taskBar = taskBar;
            _windowContainer = windowContainer;
            _applicationIconContainer = applicationIconContainer;
            InitializeTaskBarItem(applicationData);
            _applicationIconContainer.AddIcon(this);
        }

        private void InitializeTaskBarItem(ProgramResource applicationData)
        {
            if (_taskBarItem != null)
            {
                _taskBarItem.QueueFree();
            }
            _label.Text = applicationData.ProgramName;
            _icon.Texture = applicationData.Icon;
            Window window = applicationData.WindowScene.Instance<Window>();
            window.SetTitle(applicationData.WindowTitle);
            _taskBarItem = new TaskBarItem();
            _taskBarItem.BindToWindow(window);
        }

        public void Select()
        {
            _selectedEffect?.Show();
        }

        public void Deselect()
        {
            _selectedEffect?.Hide();
        }

        void IApplicationIcon.Launch()
        {
            Launch();
        }

        public Control GetIcon()
        {
            return this;
        }

        private void Launch()
        {
            _taskBar.AddTask(_taskBarItem);
            _windowContainer.AddWindow(_taskBarItem.Window);
            _taskBarItem.Window.ChangeWindowState(WindowState.Opened);
        }
        
        public override void _Pressed()
        {
            base._Pressed();
            if (_applicationIconContainer.IsSelected(this))
            {
                Launch();
                _applicationIconContainer.Deselect();
            }
            else
            {
                _applicationIconContainer.Select(this);
            }
        }
    }
}