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

public class PlayerView : MonoBehaviour
{
    public BasicCharacterController controller;
    public SkeletonAnimation skeletonAnimation;

    [Header("Animations")]
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset run;
    public AnimationReferenceAsset sliding;

    private CharacterStates _previousState;
    [SerializeField]
    private MeshRenderer _pursuerRenderer;
    [SerializeField]
    private Color32 _startSlidingColor;
    private Color32 _endSlidingColor;

    private void Start()
    {
        if (_pursuerRenderer != null)
        {
            _endSlidingColor = _pursuerRenderer.material.color;
        }
    }

    private void Update()
    {
        if (controller == null || skeletonAnimation == null) return;

        if ((skeletonAnimation.skeleton.ScaleX < 0) != controller.isFacingLeft)
            // check if player skeleton should be turned
        {
            TurnModel(controller.isFacingLeft);
        }

        if (_previousState != controller.currentState)
        {
            PlayNewAnimation();
        }

        if (_pursuerRenderer != null)
        {
            if (controller.isSliding && _previousState != CharacterStates.Sliding)
            // starting sliding
            {
                _pursuerRenderer.material.SetColor("_Color", _startSlidingColor);
            }
            else if (!controller.isSliding && _previousState == CharacterStates.Sliding)
            // ended Sliding
            {
                _pursuerRenderer.material.SetColor("_Color", _endSlidingColor);
            }
        }

        _previousState = controller.currentState;
    }

    private void PlayNewAnimation()
    {
        Spine.Animation toBePlayed;

        switch (controller.currentState)
        {
            case CharacterStates.Idle:
                toBePlayed = idle;
                break;
            case CharacterStates.Running:
                toBePlayed = run;
                break;
            case CharacterStates.Sliding:
                toBePlayed = sliding;
                break;
            default:
                Debug.LogError("Wrong state!");
                return;
        }

        skeletonAnimation.AnimationState.SetAnimation(0, toBePlayed, true);
    }

    private void TurnModel(bool isFacingLeft)
    {
        skeletonAnimation.Skeleton.ScaleX = isFacingLeft ? -1f : 1f;
    }
}
