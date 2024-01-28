using System;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class DesktopAppIconInitializer : Node
    {
        [Export] private NodePath _desktopIconContainerPath = new NodePath(".");
        private ApplicationIconContainer _desktopIconContainer;
        [Export] private NodePath _windowContainerPath = new NodePath(".");
        private WindowContainer _windowContainer;
        [Export] private NodePath _taskBarPath = new NodePath(".");
        private TaskBar _taskBar;


        public override void _Ready()
        {
            base._Ready();
            _desktopIconContainer = GetNode<ApplicationIconContainer>(_desktopIconContainerPath);
            if (_desktopIconContainer == null)
            {
                throw new Exception(
                    $"{nameof(DesktopAppIconInitializer)} {Name} failed to find {nameof(ApplicationIconContainer)} node at path {_desktopIconContainerPath}");
            }
            _windowContainer = GetNode<WindowContainer>(_windowContainerPath);
            if (_desktopIconContainer == null)
            {
                throw new Exception(
                    $"{nameof(DesktopAppIconInitializer)} {Name} failed to find {nameof(WindowContainer)} node at path {_windowContainerPath}");
            }
            _taskBar = GetNode<TaskBar>(_taskBarPath);
            if (_desktopIconContainer == null)
            {
                throw new Exception(
                    $"{nameof(DesktopAppIconInitializer)} {Name} failed to find {nameof(TaskBar)} node at path {_taskBarPath}");
            }
            foreach (ApplicationIcon applicationIcon in SearchNodeType.FindChildrenOfType<ApplicationIcon>(
                         _desktopIconContainer.IconContainer, -1))
            {
                applicationIcon.Initialize(_windowContainer, _taskBar, _desktopIconContainer);
            }
        }
    }
}
