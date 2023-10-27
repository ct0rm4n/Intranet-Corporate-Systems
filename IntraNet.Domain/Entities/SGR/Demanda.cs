using System;

namespace IntraNet.Domain.Entities.SGR
{
    public class Demanda
    {
        public int DemandaId { get; set; }
        public int ReuniaoId { get; set; }
        public int ItemAssuntoId { get; set; }
        public string Situacao { get; set; }
        public string Oque { get; set; }
        public string Como { get; set; }
        public string Porque { get; set; }
        public string Onde  { get; set; }
        public DateTime Quando { get; set; }
        public string Quanto { get; set; }
        public string Quem { get; set; }
        public DateTime InseridoEm { get; set; }
        public string QuemInseriu { get; set; }
        public bool Delete { get; set; }
        public virtual ItemAssunto itemassunto { get; set; }
        public virtual Reuniao reuniao { get; set; }
        public virtual string[] Demandado { get; set; }

    }
}
