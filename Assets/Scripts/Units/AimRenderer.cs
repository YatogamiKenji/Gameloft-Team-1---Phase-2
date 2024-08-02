using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class AimRenderer : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        public LineRenderer AimLineRenderer
        {
            get { return lineRenderer; }
            set { lineRenderer = value; }
        }
        public SpriteRenderer AimSpriteRenderer
        {
            get { return spriteRenderer; }
            set { spriteRenderer = value; }
        }
        public void DisableAll()
        {
            lineRenderer.enabled = false;
            spriteRenderer.enabled = false;
        }
    }

}
