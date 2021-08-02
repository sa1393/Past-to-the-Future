using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface TimeObjectInterface
{
    IEnumerator TimeStop();
    IEnumerator TimeFast();
    IEnumerator TimeSlow();
}
