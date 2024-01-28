using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class ApplicationIconContainer : Node
    {
        private readonly Dictionary<IApplicationIcon, IApplicationIcon> _applicationIcons =
            new Dictionary<IApplicationIcon, IApplicationIcon>();

        [Export] private NodePath _iconContainerPath = new NodePath(".");
        private Container _iconContainer;
        private IApplicationIcon _selectedIcon = null;

        public override void _Ready()
        {
            base._Ready();
            _iconContainer = GetNode<Container>(_iconContainerPath);
            if (_iconContainer == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(Container)} for {nameof(ApplicationIconContainer)} {Name} at path {_iconContainerPath}!");
            }
        }

        public void AddIcon(IApplicationIcon applicationIcon)
        {
            if (_applicationIcons.ContainsKey(applicationIcon))
            {
                return;
            }

            _applicationIcons[applicationIcon] = applicationIcon;
            ReplaceParent.Replace(applicationIcon.GetIcon(), _iconContainer);
        }

        public void RemoveIcon(IApplicationIcon applicationIcon)
        {
            if (!_applicationIcons.Remove(applicationIcon))
            {
                return;
            }

            ReplaceParent.Replace(applicationIcon.GetIcon(), null);
        }

        public bool IsSelected(IApplicationIcon applicationIcon)
        {
            return _selectedIcon == applicationIcon;
        }

        public void Select(IApplicationIcon applicationIcon)
        {
            Deselect();
            applicationIcon?.Select();
            _selectedIcon = applicationIcon;
        }

        public void Deselect()
        {
            _selectedIcon?.Deselect();
            _selectedIcon = null;
        }
    }
}