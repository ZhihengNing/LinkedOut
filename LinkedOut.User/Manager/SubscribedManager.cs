using LinkedOut.DB;
using LinkedOut.User.Domain.Bo;

namespace LinkedOut.User.Manager;

public class SubscribedManager
{
    private readonly LinkedOutContext _context;

    public SubscribedManager(LinkedOutContext context)
    {
        _context = context;
    }

    public SubscribedState GetRelation(int firstUserId, int secondUserId)
    {
        if (firstUserId == secondUserId)
        {
            return SubscribedState.Same;
        }
        var relation = _context.Subscribeds.Select(o => o)
            .SingleOrDefault(o => o.FirstUserId == o.SecondUserId);
        return relation == null ? SubscribedState.NoSubscribed : SubscribedState.SubScribed;
    }
}