public class Player
{
    /// <summary>
    /// Экземпляр Player
    /// </summary>
    private static Player instance;

    /// <summary>
    /// Получение единственного экземпляра Player
    /// </summary>
    public static Player Instance
    {
        get
        {
            if (instance == null)
                instance = new Player();
            return instance;
        }
    }

    public Rod SelectedRod { get; private set; }

    private Player()
    {
        instance = this;
        SelectedRod = ItemManager.Instance.GetSelectedRod();
    }
}