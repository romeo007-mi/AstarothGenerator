public class AstarothSemaphore
{
    private int x = 1;

    public bool IsResourceAvailable()
    {
        return x == 1;
    }

    public bool IsResourceNotAvailable()
    {
        return x == 0;
    }

    public bool LockResource()
    {
        if (x == 1)
        {
            x = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnlockResource()
    {
        if (x == 0)
        {
            x = 1;
        }
    }
}