/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated January 1, 2020. Replaces all prior versions.
 *
 * Copyright (c) 2013-2020, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using Spine.Unity;
using UnityEngine;

public enum CharacterStates
{
    Idle,
    Running,
    Sliding
}

public class BasicCharacterController : MonoBehaviour
{
    public CharacterStates currentState;
    public bool isFacingLeft;
    [Range(-1f, 1f)]
    public float currentSpeed;
    public float speedMultiplier = 5;
    public bool isSliding = false;

    private void Update()
    {
        currentSpeed = Input.GetAxisRaw("Horizontal");

        // Are we sliding?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f, LayerMask.GetMask("Platforms"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Slope"))
            {
                Debug.Log("Yep! It`s a slope.");
                currentState = CharacterStates.Sliding;
                if (isSliding == false) isSliding = true;
            }
            else
            {
                currentState = CharacterStates.Idle;
                isSliding = false;
            }
        }
        if (currentState == CharacterStates.Sliding)
        {
            // if we`re sliding, slide down without control
            Vector3 deltaDisplacement;
            if (isFacingLeft)
            {
                deltaDisplacement = speedMultiplier * Time.deltaTime * -transform.right;
            }
            else
            {
                deltaDisplacement = speedMultiplier * Time.deltaTime * transform.right;
            }
            transform.position += deltaDisplacement;
        }
        else
        {
            if (currentSpeed != 0f)
            {
                isFacingLeft = (currentSpeed < 0f) ? true : false;
                currentState = CharacterStates.Running;
                Vector3 deltaDisplacement;
                if (isFacingLeft)
                {
                    deltaDisplacement = speedMultiplier * Time.deltaTime * -transform.right;
                }
                else
                {
                    deltaDisplacement = speedMultiplier * Time.deltaTime * transform.right;
                }
                transform.position += deltaDisplacement;
            }
            else
            {
                currentState = CharacterStates.Idle;
            }
        }
    }
}
