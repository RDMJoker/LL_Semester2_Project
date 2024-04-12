using UnityEngine;

namespace KI
{
    public class TargetComponent
    {
        Vector3 position;
        Transform targetTransform;
        public Vector3 TargetPosition => targetTransform == null ? position : targetTransform.position;
        
        public void SetTarget(Transform _target)
        {
            targetTransform = _target;
        }

        public void SetPoint(Vector3 _point)
        {
            position = _point;
        }
    }
}

/*
    XX NUR XOOXXXXXX XOXXXX XX EINE XXXXOX 
    XXXXXXXX XXX XXXX XXXXXX XX OXXXX XXXXXXX WAHRHEIT XXXXXXOXX
        
        chueek
        
        
    
        "NUR EINE WAHRHEIT"
    "Die versunkene Kammer hält Rätsel bereit. Es wurden Vorbereitungen getroffen. 68 Schlangen wurden 20 Fragen gestellt. Sie alle werden in einer Spirale versinken."
    "Die versunkene Kammer hält Rätsel bereit. Es wurden Vorbereitungen getroffen. Einer geht hinein, aber niemand kommt hinaus."
    "Die versunkene Kammer hält Rätsel bereit. Es wurden Vorbereitungen getroffen. Worte sind nur Schall und Rauch."
    "Die versunkene Kammer hält Rätsel bereit. Es wurden Vorbereitungen getroffen. Die Dame und der Herr der Rätsel haben alles von langer Hand geplant." */