namespace bbt.service.notification.ui.Component.Modal
{ 
    public partial class BaseLoadingModal: BaseModalCode
    {
        public BaseModal baseModal { get; set; }

        public void Open()
        {
            baseModal.Open();
        }

        public void Close()
        {
            baseModal.Close();
        }
    }
}
