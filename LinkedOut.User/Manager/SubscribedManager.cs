using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Enum;

namespace LinkedOut.User.Manager;

public class SubscribedManager
{
    private readonly LinkedOutContext _context;

    public SubscribedManager(LinkedOutContext context)
    {
        _context = context;
    }

    public (SubscribedState SubScribed, Subscribed? relation) GetRelation(int firstUserId, int secondUserId)
    {
        if (firstUserId == secondUserId)
        {
            return (SubscribedState.Same,null);
        }

        var relation = _context.Subscribeds.Select(o => o)
            .SingleOrDefault(o => o.FirstUserId == o.SecondUserId);
        return relation == null ? (SubscribedState.NoSubscribed, null) : (SubscribedState.SubScribed, relation);
    }
}