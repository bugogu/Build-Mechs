// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("NZ7RdFPiPTGdgqnFQEL3Ci2VT221DaOBxeSCWhQQbW5a1Unedb0ApWe6ODn/DLo9TG7CT/YiMNE5eBaaLMqQBGPUcmpV8bg8XE1Li+zRh5IoL1LXyhV/gnZNNvTErYHjoWP3fzY8Xb3682203M+cTukgpfaoSidtvw2Orb+CiYalCccJeIKOjo6Kj4yu4RcmTUy21jeJVvk73GJUTK4T9GV9NhXLTZALBgsSF+k5WFchvx4ike/lO+gDuUosVmK7reKO8swuTL3htqcn4RCreDUysZH7Kimt3Xheog2OgI+/DY6FjQ2Ojo8gFcdUaeUkMmRPUfnyNIXoXlE9z9JrLW3BGCKuT+1KKSIiqr4NVe6yhWrkfwsjLE93+n8lP3I+Jo2Mjo+O");
        private static int[] order = new int[] { 3,6,8,12,13,11,12,10,8,13,13,12,12,13,14 };
        private static int key = 143;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
