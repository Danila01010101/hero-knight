using System;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Vector2 _defaultDirection = Vector2.zero;
    private Vector2 _upDirection = Vector2.up;
    private Vector2 _downDirection = Vector2.down;
    private Vector2 _rightDirection = Vector2.right;
    private Vector2 _leftDirection = Vector2.left;
    private Vector2 _newDirection;
    private Action<Vector2> _moveAction;

    public bool HasDirection => _newDirection != Vector2.zero;

    public enum Direction { Right, Left, Up, Down}

    public MoveCommand(Action<Vector2> moveAction)
    {
        _moveAction = moveAction;
    }

    public void AddDirection(Direction direction)
    {
        _newDirection += direction switch
        {
            Direction.Left => _leftDirection,
            Direction.Right => _rightDirection,
            Direction.Up => _upDirection,
            Direction.Down => _downDirection,
            _ => throw new NotImplementedException()
        };
    }

    public void Execute()
    {
        _moveAction?.Invoke(_newDirection);
        _newDirection = _defaultDirection;
    }
}