using System;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
	public class ApplicationIcon : Button, IApplicationIcon
	{
		[Export] private NodePath _labelPath = new NodePath(".");
		private Label _label;

		private Label AppLabel
		{
			get
			{
				if (_label == null)
				{
					_label = GetNode<Label>(_labelPath);
					if (_label == null)
					{
						throw new Exception(
							$"Failed to find a {nameof(Label)} for {nameof(ApplicationIcon)} {Name} with path {_labelPath}!");
					}
				}

				return _label;
			}
		}

		[Export] private NodePath _iconPath = new NodePath(".");
		private TextureRect _icon;

		private TextureRect AppIcon
		{
			get
			{
				if (_icon == null)
				{
					_icon = GetNode<TextureRect>(_iconPath);
					if (_label == null)
					{
						throw new Exception(
							$"Failed to find a {nameof(TextureRect)} for {nameof(ApplicationIcon)} {Name} with path {_iconPath}!");
					}
				}

				return _icon;
			}
		}

		[Export] private NodePath _selectedEffectPath = new NodePath(".");
		private Control _selectedEffect;

		private Control SelectedEffect
		{
			get
			{
				if (_selectedEffect == null)
				{
					if (GetNode(_selectedEffectPath) is Control controlNode)
					{
						_selectedEffect = controlNode;
					}
				}

				return _selectedEffect;
			}
		}

		private TaskBar _taskBar;
		private WindowContainer _windowContainer;
		private TaskBarItem _taskBarItem;
		private ApplicationIconContainer _applicationIconContainer;
		[Export] private ProgramResource _applicationData = null;


		public void Initialize(WindowContainer windowContainer, TaskBar taskBar,
			ApplicationIconContainer applicationIconContainer)
		{
			_taskBar = taskBar;
			_windowContainer = windowContainer;
			_applicationIconContainer = applicationIconContainer;
			InitializeTaskBarItem(_applicationData);
			_applicationIconContainer.AddIcon(this);
		}

		private void InitializeTaskBarItem(ProgramResource applicationData)
		{
			if (_taskBarItem != null)
			{
				_taskBarItem.QueueFree();
			}

			if (applicationData == null || applicationData.ProgramName == null || applicationData.ProgramName.Length == 0)
			{
				AppLabel.Text = "";
				AppIcon.Texture = null;
				return;
			}

			AppLabel.Text = applicationData.ProgramName;
			AppIcon.Texture = applicationData.Icon;
			Window window = applicationData.WindowScene.Instance<Window>();
			window.SetTitle(applicationData.WindowTitle);
			_taskBarItem = applicationData.TaskBarItemScene.Instance<TaskBarItem>();
			_taskBarItem.BindToWindow(window);
			if (SearchNodeType.FindChildOfType<TextureRect>(_taskBarItem) is TextureRect textureRect)
			{
				textureRect.Texture = AppIcon.Texture;
			}
		}

		public void Select()
		{
			SelectedEffect?.Show();
		}

		public void Deselect()
		{
			SelectedEffect?.Hide();
		}
		public Control GetIcon()
		{
			return this;
		}

		public void Launch()
		{
			_taskBar.AddTask(_taskBarItem);
			_windowContainer.AddWindow(_taskBarItem.Window);
			_taskBarItem.Window.ChangeWindowState(WindowState.Opened);
		}

		public override void _Pressed()
		{
			base._Pressed();
			GD.Print($"{Name} pressed for icon with label {AppLabel.Text}");
			if (_applicationIconContainer.IsSelected(this))
			{
				if (_applicationData != null)
				{
					GD.Print($"{Name} launching program {_applicationData?.ResourcePath}");
					Launch();
				}

				_applicationIconContainer.Deselect();
			}
			else
			{
				_applicationIconContainer.Select(this);
			}
		}
	}
}
