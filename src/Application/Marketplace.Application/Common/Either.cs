namespace Marketplace.Application.Common;

public class Either<TLeft, TRight>
{
    private readonly TRight _right;
    private readonly TLeft _left;

    private readonly bool IsLeft;

    public Either(TLeft left)
    {
        _left = left;
        IsLeft = true;
    }

    public Either(TRight right)
    {
        _right = right;
        IsLeft = false;
    }

    public T Match<T>(Func<TLeft, T> left, Func<TRight, T> right)
        => IsLeft ? left(_left) : right(_right);
}