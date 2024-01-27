namespace DefaultNamespace
{
    public class AtkButton : BaseButton
    {
        protected override void Onclick()
        {
            PlayerController.Instance.OnClickAttack();
        }
    }
}