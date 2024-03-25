using Photon.Pun;

namespace Fight.Person
{
    public class EventSync: IPunObservable
    {
        public GameEvent GameEvent { get; set; }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(GameEvent);
            }
            else
            {
                GameEvent = (GameEvent)stream.ReceiveNext();
            }
        }
    }
}