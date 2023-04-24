namespace judo_univ_rennes.Configurations
{
    /// <summary>
    /// Class de configuration pour les URL base d'api
    /// </summary>
    public class BaseAddress
    {
        public string Protocole { get; set; }
        public string Host { get; set; } = null;
        public string Port { get; set; } = null;
    }
}
