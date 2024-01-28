using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class TaskBar : Control
    {
        private ITaskBarItem _selected;
        private readonly Dictionary<ITaskBarItem, ITaskBarItem> _tasks = new Dictionary<ITaskBarItem, ITaskBarItem>();
        [Export] private NodePath _taskBarIconContainerPath = new NodePath(".");
        private Control _taskBarIconContainer;

        public override void _Ready()
        {
            base._Ready();
            _taskBarIconContainer = GetNode<Control>(_taskBarIconContainerPath);
            if (_taskBarIconContainer == null)
            {
                throw new Exception($"Failed to find TaskNodeContainer control for {nameof(TaskBar)} {Name}!");
            }
        }

        public void AddTask(ITaskBarItem newItem)
        {
            if (_tasks.ContainsKey(newItem))
            {
                return;
            }

            _tasks[newItem] = newItem;
            ReplaceParent.Replace(newItem.GetTaskIcon(), _taskBarIconContainer);
        }

        public void RemoveTask(ITaskBarItem removedItem)
        {
            if (_tasks.Remove(removedItem))
            {
                ReplaceParent.Replace(removedItem.GetTaskIcon(), null);
            }
        }

        public void Select(ITaskBarItem taskBarItem)
        {
            _selected?.DeSelect();
            _selected = taskBarItem;
            if (taskBarItem == null)
            {
                return;
            }
            AddTask(taskBarItem);
            ReplaceParent.Replace(_selected.GetTaskIcon(), null);
            ReplaceParent.Replace(_selected.GetTaskIcon(), _taskBarIconContainer);
        }
    }
}