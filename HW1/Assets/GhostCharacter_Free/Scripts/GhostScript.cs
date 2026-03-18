using UnityEngine;

namespace Sample
{
    /// <summary>
    /// Ödev için sadeleştirilmiş player kontrolü:
    /// - WASD/Arrow ile hareket (kameraya göre)
    /// - CharacterController ile yerçekimi
    /// - Opsiyonel: Animator'da "isMoving" bool parametresini sürer
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class GhostScript : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float moveSpeed = 4f;
        [SerializeField] float turnSpeed = 10f;
        [SerializeField] float gravity = -20f;

        [Header("Animation (optional)")]
        [SerializeField] Animator animator;
        [SerializeField] string isMovingParam = "isMoving";
        bool hasIsMovingParam;

        CharacterController controller;
        Vector3 velocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (animator == null) animator = GetComponent<Animator>();

            hasIsMovingParam = animator != null && HasBoolParameter(animator, isMovingParam);
        }

        void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = GetCameraRelativeDirection(h, v);

            // Hareket
            controller.Move(moveDir * (moveSpeed * Time.deltaTime));

            // Yüzünü hareket yönüne çevir
            if (moveDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            }

            // Yerçekimi
            if (controller.isGrounded && velocity.y < 0f)
                velocity.y = -1f;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Basit animasyon flag
            if (animator != null && hasIsMovingParam)
                animator.SetBool(isMovingParam, moveDir.sqrMagnitude > 0.0001f);
        }

        static bool HasBoolParameter(Animator anim, string paramName)
        {
            if (anim == null || string.IsNullOrEmpty(paramName)) return false;
            foreach (var p in anim.parameters)
            {
                if (p.type == AnimatorControllerParameterType.Bool && p.name == paramName)
                    return true;
            }
            return false;
        }

        Vector3 GetCameraRelativeDirection(float h, float v)
        {
            Vector3 input = new Vector3(h, 0f, v);
            if (input.sqrMagnitude <= 0.0001f)
                return Vector3.zero;

            var cam = Camera.main;
            if (cam == null)
                return input.normalized;

            Vector3 forward = cam.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = cam.transform.right;
            right.y = 0f;
            right.Normalize();

            return (forward * v + right * h).normalized;
        }
    }
}