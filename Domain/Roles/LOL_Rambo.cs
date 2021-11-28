namespace AliceMafia
{
    public class LOL_Rambo : IRole
    {
        //вы не поверите, короче он гранату кидает и убивает челов с обеих сторон
        public string Name => "Рэмбо";

        public int ThrowGrenade()
        {
            return 1337;
        }
    }
}