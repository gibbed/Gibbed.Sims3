#include <windows.h>
#include "declarations.h"

HINSTANCE h_original;

BOOL WINAPI h_CryptVerifySignature(HCRYPTHASH hHash, BYTE *pbSignature, DWORD dwSigLen, HCRYPTKEY hPubKey, LPCTSTR sDescription, DWORD dwFlags)
{
	return TRUE;
}

void HookAdvApi()
{
	HINSTANCE dll = LoadLibrary("advapi32");
	if (dll == NULL)
	{
		return;
	}
	
	unsigned char *cvs = (unsigned char *)GetProcAddress(dll, "CryptVerifySignatureA");
	
	unsigned char jump[5];
	jump[0] = 0xE9;
	*(unsigned int *)(jump + 1) = (unsigned int)&h_CryptVerifySignature - (unsigned int)cvs - 5;
	
	DWORD old, junk;
	VirtualProtect((void *)cvs, 5, PAGE_EXECUTE_READWRITE, &old);
	memcpy((void *)cvs, (void*)jump, 5);
	VirtualProtect((void *)cvs, 12, old, &junk);
	FlushInstructionCache(GetCurrentProcess(), (void *)cvs, 5);
}

void LoadStubs(HINSTANCE dll);
BOOL APIENTRY DllMain(HINSTANCE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
		{
			char path[MAX_PATH];
			GetSystemDirectory(path, MAX_PATH);
			strcat(path, "\\d3dx9_31.dll");

			h_original = LoadLibrary(path);
			if (h_original == NULL)
			{
				return FALSE;
			}
			
			HookAdvApi();
			LoadStubs(h_original);
			break;
		}

		case DLL_PROCESS_DETACH:
		{
			FreeLibrary(h_original);
			break;
		}
	}

	return TRUE;
}

void LoadStubs(HINSTANCE dll)
{
    p_D3DXAssembleShader = (APIWRAPPER)GetProcAddress(h_original, "D3DXAssembleShader");
    p_D3DXAssembleShaderFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXAssembleShaderFromFileA");
    p_D3DXAssembleShaderFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXAssembleShaderFromFileW");
    p_D3DXAssembleShaderFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXAssembleShaderFromResourceA");
    p_D3DXAssembleShaderFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXAssembleShaderFromResourceW");
    p_D3DXBoxBoundProbe = (APIWRAPPER)GetProcAddress(h_original, "D3DXBoxBoundProbe");
    p_D3DXCheckCubeTextureRequirements = (APIWRAPPER)GetProcAddress(h_original, "D3DXCheckCubeTextureRequirements");
    p_D3DXCheckTextureRequirements = (APIWRAPPER)GetProcAddress(h_original, "D3DXCheckTextureRequirements");
    p_D3DXCheckVersion = (APIWRAPPER)GetProcAddress(h_original, "D3DXCheckVersion");
    p_D3DXCheckVolumeTextureRequirements = (APIWRAPPER)GetProcAddress(h_original, "D3DXCheckVolumeTextureRequirements");
    p_D3DXCleanMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXCleanMesh");
    p_D3DXColorAdjustContrast = (APIWRAPPER)GetProcAddress(h_original, "D3DXColorAdjustContrast");
    p_D3DXColorAdjustSaturation = (APIWRAPPER)GetProcAddress(h_original, "D3DXColorAdjustSaturation");
    p_D3DXCompileShader = (APIWRAPPER)GetProcAddress(h_original, "D3DXCompileShader");
    p_D3DXCompileShaderFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCompileShaderFromFileA");
    p_D3DXCompileShaderFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCompileShaderFromFileW");
    p_D3DXCompileShaderFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCompileShaderFromResourceA");
    p_D3DXCompileShaderFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCompileShaderFromResourceW");
    p_D3DXComputeBoundingBox = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeBoundingBox");
    p_D3DXComputeBoundingSphere = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeBoundingSphere");
    p_D3DXComputeIMTFromPerTexelSignal = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeIMTFromPerTexelSignal");
    p_D3DXComputeIMTFromPerVertexSignal = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeIMTFromPerVertexSignal");
    p_D3DXComputeIMTFromSignal = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeIMTFromSignal");
    p_D3DXComputeIMTFromTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeIMTFromTexture");
    p_D3DXComputeNormalMap = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeNormalMap");
    p_D3DXComputeNormals = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeNormals");
    p_D3DXComputeTangent = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeTangent");
    p_D3DXComputeTangentFrame = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeTangentFrame");
    p_D3DXComputeTangentFrameEx = (APIWRAPPER)GetProcAddress(h_original, "D3DXComputeTangentFrameEx");
    p_D3DXConcatenateMeshes = (APIWRAPPER)GetProcAddress(h_original, "D3DXConcatenateMeshes");
    p_D3DXConvertMeshSubsetToSingleStrip = (APIWRAPPER)GetProcAddress(h_original, "D3DXConvertMeshSubsetToSingleStrip");
    p_D3DXConvertMeshSubsetToStrips = (APIWRAPPER)GetProcAddress(h_original, "D3DXConvertMeshSubsetToStrips");
    p_D3DXCreateAnimationController = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateAnimationController");
    p_D3DXCreateBox = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateBox");
    p_D3DXCreateBuffer = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateBuffer");
    p_D3DXCreateCompressedAnimationSet = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCompressedAnimationSet");
    p_D3DXCreateCubeTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTexture");
    p_D3DXCreateCubeTextureFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromFileA");
    p_D3DXCreateCubeTextureFromFileExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromFileExA");
    p_D3DXCreateCubeTextureFromFileExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromFileExW");
    p_D3DXCreateCubeTextureFromFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromFileInMemory");
    p_D3DXCreateCubeTextureFromFileInMemoryEx = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromFileInMemoryEx");
    p_D3DXCreateCubeTextureFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromFileW");
    p_D3DXCreateCubeTextureFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromResourceA");
    p_D3DXCreateCubeTextureFromResourceExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromResourceExA");
    p_D3DXCreateCubeTextureFromResourceExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromResourceExW");
    p_D3DXCreateCubeTextureFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCubeTextureFromResourceW");
    p_D3DXCreateCylinder = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateCylinder");
    p_D3DXCreateEffect = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffect");
    p_D3DXCreateEffectCompiler = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectCompiler");
    p_D3DXCreateEffectCompilerFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectCompilerFromFileA");
    p_D3DXCreateEffectCompilerFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectCompilerFromFileW");
    p_D3DXCreateEffectCompilerFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectCompilerFromResourceA");
    p_D3DXCreateEffectCompilerFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectCompilerFromResourceW");
    p_D3DXCreateEffectEx = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectEx");
    p_D3DXCreateEffectFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromFileA");
    p_D3DXCreateEffectFromFileExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromFileExA");
    p_D3DXCreateEffectFromFileExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromFileExW");
    p_D3DXCreateEffectFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromFileW");
    p_D3DXCreateEffectFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromResourceA");
    p_D3DXCreateEffectFromResourceExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromResourceExA");
    p_D3DXCreateEffectFromResourceExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromResourceExW");
    p_D3DXCreateEffectFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectFromResourceW");
    p_D3DXCreateEffectPool = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateEffectPool");
    p_D3DXCreateFontA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateFontA");
    p_D3DXCreateFontIndirectA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateFontIndirectA");
    p_D3DXCreateFontIndirectW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateFontIndirectW");
    p_D3DXCreateFontW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateFontW");
    p_D3DXCreateFragmentLinker = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateFragmentLinker");
    p_D3DXCreateKeyframedAnimationSet = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateKeyframedAnimationSet");
    p_D3DXCreateLine = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateLine");
    p_D3DXCreateMatrixStack = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateMatrixStack");
    p_D3DXCreateMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateMesh");
    p_D3DXCreateMeshFVF = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateMeshFVF");
    p_D3DXCreateNPatchMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateNPatchMesh");
    p_D3DXCreatePMeshFromStream = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePMeshFromStream");
    p_D3DXCreatePRTBuffer = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePRTBuffer");
    p_D3DXCreatePRTBufferTex = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePRTBufferTex");
    p_D3DXCreatePRTCompBuffer = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePRTCompBuffer");
    p_D3DXCreatePRTEngine = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePRTEngine");
    p_D3DXCreatePatchMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePatchMesh");
    p_D3DXCreatePolygon = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreatePolygon");
    p_D3DXCreateRenderToEnvMap = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateRenderToEnvMap");
    p_D3DXCreateRenderToSurface = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateRenderToSurface");
    p_D3DXCreateSPMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateSPMesh");
    p_D3DXCreateSkinInfo = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateSkinInfo");
    p_D3DXCreateSkinInfoFVF = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateSkinInfoFVF");
    p_D3DXCreateSkinInfoFromBlendedMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateSkinInfoFromBlendedMesh");
    p_D3DXCreateSphere = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateSphere");
    p_D3DXCreateSprite = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateSprite");
    p_D3DXCreateTeapot = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTeapot");
    p_D3DXCreateTextA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextA");
    p_D3DXCreateTextW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextW");
    p_D3DXCreateTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTexture");
    p_D3DXCreateTextureFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromFileA");
    p_D3DXCreateTextureFromFileExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromFileExA");
    p_D3DXCreateTextureFromFileExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromFileExW");
    p_D3DXCreateTextureFromFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromFileInMemory");
    p_D3DXCreateTextureFromFileInMemoryEx = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromFileInMemoryEx");
    p_D3DXCreateTextureFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromFileW");
    p_D3DXCreateTextureFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromResourceA");
    p_D3DXCreateTextureFromResourceExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromResourceExA");
    p_D3DXCreateTextureFromResourceExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromResourceExW");
    p_D3DXCreateTextureFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureFromResourceW");
    p_D3DXCreateTextureGutterHelper = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureGutterHelper");
    p_D3DXCreateTextureShader = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTextureShader");
    p_D3DXCreateTorus = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateTorus");
    p_D3DXCreateVolumeTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTexture");
    p_D3DXCreateVolumeTextureFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromFileA");
    p_D3DXCreateVolumeTextureFromFileExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromFileExA");
    p_D3DXCreateVolumeTextureFromFileExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromFileExW");
    p_D3DXCreateVolumeTextureFromFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromFileInMemory");
    p_D3DXCreateVolumeTextureFromFileInMemoryEx = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromFileInMemoryEx");
    p_D3DXCreateVolumeTextureFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromFileW");
    p_D3DXCreateVolumeTextureFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromResourceA");
    p_D3DXCreateVolumeTextureFromResourceExA = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromResourceExA");
    p_D3DXCreateVolumeTextureFromResourceExW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromResourceExW");
    p_D3DXCreateVolumeTextureFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXCreateVolumeTextureFromResourceW");
    p_D3DXDebugMute = (APIWRAPPER)GetProcAddress(h_original, "D3DXDebugMute");
    p_D3DXDeclaratorFromFVF = (APIWRAPPER)GetProcAddress(h_original, "D3DXDeclaratorFromFVF");
    p_D3DXDisassembleEffect = (APIWRAPPER)GetProcAddress(h_original, "D3DXDisassembleEffect");
    p_D3DXDisassembleShader = (APIWRAPPER)GetProcAddress(h_original, "D3DXDisassembleShader");
    p_D3DXFVFFromDeclarator = (APIWRAPPER)GetProcAddress(h_original, "D3DXFVFFromDeclarator");
    p_D3DXFileCreate = (APIWRAPPER)GetProcAddress(h_original, "D3DXFileCreate");
    p_D3DXFillCubeTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXFillCubeTexture");
    p_D3DXFillCubeTextureTX = (APIWRAPPER)GetProcAddress(h_original, "D3DXFillCubeTextureTX");
    p_D3DXFillTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXFillTexture");
    p_D3DXFillTextureTX = (APIWRAPPER)GetProcAddress(h_original, "D3DXFillTextureTX");
    p_D3DXFillVolumeTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXFillVolumeTexture");
    p_D3DXFillVolumeTextureTX = (APIWRAPPER)GetProcAddress(h_original, "D3DXFillVolumeTextureTX");
    p_D3DXFilterTexture = (APIWRAPPER)GetProcAddress(h_original, "D3DXFilterTexture");
    p_D3DXFindShaderComment = (APIWRAPPER)GetProcAddress(h_original, "D3DXFindShaderComment");
    p_D3DXFloat16To32Array = (APIWRAPPER)GetProcAddress(h_original, "D3DXFloat16To32Array");
    p_D3DXFloat32To16Array = (APIWRAPPER)GetProcAddress(h_original, "D3DXFloat32To16Array");
    p_D3DXFrameAppendChild = (APIWRAPPER)GetProcAddress(h_original, "D3DXFrameAppendChild");
    p_D3DXFrameCalculateBoundingSphere = (APIWRAPPER)GetProcAddress(h_original, "D3DXFrameCalculateBoundingSphere");
    p_D3DXFrameDestroy = (APIWRAPPER)GetProcAddress(h_original, "D3DXFrameDestroy");
    p_D3DXFrameFind = (APIWRAPPER)GetProcAddress(h_original, "D3DXFrameFind");
    p_D3DXFrameNumNamedMatrices = (APIWRAPPER)GetProcAddress(h_original, "D3DXFrameNumNamedMatrices");
    p_D3DXFrameRegisterNamedMatrices = (APIWRAPPER)GetProcAddress(h_original, "D3DXFrameRegisterNamedMatrices");
    p_D3DXFresnelTerm = (APIWRAPPER)GetProcAddress(h_original, "D3DXFresnelTerm");
    p_D3DXGatherFragments = (APIWRAPPER)GetProcAddress(h_original, "D3DXGatherFragments");
    p_D3DXGatherFragmentsFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXGatherFragmentsFromFileA");
    p_D3DXGatherFragmentsFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXGatherFragmentsFromFileW");
    p_D3DXGatherFragmentsFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXGatherFragmentsFromResourceA");
    p_D3DXGatherFragmentsFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXGatherFragmentsFromResourceW");
    p_D3DXGenerateOutputDecl = (APIWRAPPER)GetProcAddress(h_original, "D3DXGenerateOutputDecl");
    p_D3DXGeneratePMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXGeneratePMesh");
    p_D3DXGetDeclLength = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetDeclLength");
    p_D3DXGetDeclVertexSize = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetDeclVertexSize");
    p_D3DXGetDriverLevel = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetDriverLevel");
    p_D3DXGetFVFVertexSize = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetFVFVertexSize");
    p_D3DXGetImageInfoFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetImageInfoFromFileA");
    p_D3DXGetImageInfoFromFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetImageInfoFromFileInMemory");
    p_D3DXGetImageInfoFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetImageInfoFromFileW");
    p_D3DXGetImageInfoFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetImageInfoFromResourceA");
    p_D3DXGetImageInfoFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetImageInfoFromResourceW");
    p_D3DXGetPixelShaderProfile = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetPixelShaderProfile");
    p_D3DXGetShaderConstantTable = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetShaderConstantTable");
    p_D3DXGetShaderInputSemantics = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetShaderInputSemantics");
    p_D3DXGetShaderOutputSemantics = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetShaderOutputSemantics");
    p_D3DXGetShaderSamplers = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetShaderSamplers");
    p_D3DXGetShaderSize = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetShaderSize");
    p_D3DXGetShaderVersion = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetShaderVersion");
    p_D3DXGetVertexShaderProfile = (APIWRAPPER)GetProcAddress(h_original, "D3DXGetVertexShaderProfile");
    p_D3DXIntersect = (APIWRAPPER)GetProcAddress(h_original, "D3DXIntersect");
    p_D3DXIntersectSubset = (APIWRAPPER)GetProcAddress(h_original, "D3DXIntersectSubset");
    p_D3DXIntersectTri = (APIWRAPPER)GetProcAddress(h_original, "D3DXIntersectTri");
    p_D3DXLoadMeshFromXA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshFromXA");
    p_D3DXLoadMeshFromXInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshFromXInMemory");
    p_D3DXLoadMeshFromXResource = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshFromXResource");
    p_D3DXLoadMeshFromXW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshFromXW");
    p_D3DXLoadMeshFromXof = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshFromXof");
    p_D3DXLoadMeshHierarchyFromXA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshHierarchyFromXA");
    p_D3DXLoadMeshHierarchyFromXInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshHierarchyFromXInMemory");
    p_D3DXLoadMeshHierarchyFromXW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadMeshHierarchyFromXW");
    p_D3DXLoadPRTBufferFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadPRTBufferFromFileA");
    p_D3DXLoadPRTBufferFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadPRTBufferFromFileW");
    p_D3DXLoadPRTCompBufferFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadPRTCompBufferFromFileA");
    p_D3DXLoadPRTCompBufferFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadPRTCompBufferFromFileW");
    p_D3DXLoadPatchMeshFromXof = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadPatchMeshFromXof");
    p_D3DXLoadSkinMeshFromXof = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSkinMeshFromXof");
    p_D3DXLoadSurfaceFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromFileA");
    p_D3DXLoadSurfaceFromFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromFileInMemory");
    p_D3DXLoadSurfaceFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromFileW");
    p_D3DXLoadSurfaceFromMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromMemory");
    p_D3DXLoadSurfaceFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromResourceA");
    p_D3DXLoadSurfaceFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromResourceW");
    p_D3DXLoadSurfaceFromSurface = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadSurfaceFromSurface");
    p_D3DXLoadVolumeFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromFileA");
    p_D3DXLoadVolumeFromFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromFileInMemory");
    p_D3DXLoadVolumeFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromFileW");
    p_D3DXLoadVolumeFromMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromMemory");
    p_D3DXLoadVolumeFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromResourceA");
    p_D3DXLoadVolumeFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromResourceW");
    p_D3DXLoadVolumeFromVolume = (APIWRAPPER)GetProcAddress(h_original, "D3DXLoadVolumeFromVolume");
    p_D3DXMatrixAffineTransformation2D = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixAffineTransformation2D");
    p_D3DXMatrixAffineTransformation = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixAffineTransformation");
    p_D3DXMatrixDecompose = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixDecompose");
    p_D3DXMatrixDeterminant = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixDeterminant");
    p_D3DXMatrixInverse = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixInverse");
    p_D3DXMatrixLookAtLH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixLookAtLH");
    p_D3DXMatrixLookAtRH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixLookAtRH");
    p_D3DXMatrixMultiply = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixMultiply");
    p_D3DXMatrixMultiplyTranspose = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixMultiplyTranspose");
    p_D3DXMatrixOrthoLH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixOrthoLH");
    p_D3DXMatrixOrthoOffCenterLH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixOrthoOffCenterLH");
    p_D3DXMatrixOrthoOffCenterRH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixOrthoOffCenterRH");
    p_D3DXMatrixOrthoRH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixOrthoRH");
    p_D3DXMatrixPerspectiveFovLH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixPerspectiveFovLH");
    p_D3DXMatrixPerspectiveFovRH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixPerspectiveFovRH");
    p_D3DXMatrixPerspectiveLH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixPerspectiveLH");
    p_D3DXMatrixPerspectiveOffCenterLH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixPerspectiveOffCenterLH");
    p_D3DXMatrixPerspectiveOffCenterRH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixPerspectiveOffCenterRH");
    p_D3DXMatrixPerspectiveRH = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixPerspectiveRH");
    p_D3DXMatrixReflect = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixReflect");
    p_D3DXMatrixRotationAxis = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixRotationAxis");
    p_D3DXMatrixRotationQuaternion = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixRotationQuaternion");
    p_D3DXMatrixRotationX = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixRotationX");
    p_D3DXMatrixRotationY = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixRotationY");
    p_D3DXMatrixRotationYawPitchRoll = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixRotationYawPitchRoll");
    p_D3DXMatrixRotationZ = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixRotationZ");
    p_D3DXMatrixScaling = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixScaling");
    p_D3DXMatrixShadow = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixShadow");
    p_D3DXMatrixTransformation2D = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixTransformation2D");
    p_D3DXMatrixTransformation = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixTransformation");
    p_D3DXMatrixTranslation = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixTranslation");
    p_D3DXMatrixTranspose = (APIWRAPPER)GetProcAddress(h_original, "D3DXMatrixTranspose");
    p_D3DXOptimizeFaces = (APIWRAPPER)GetProcAddress(h_original, "D3DXOptimizeFaces");
    p_D3DXOptimizeVertices = (APIWRAPPER)GetProcAddress(h_original, "D3DXOptimizeVertices");
    p_D3DXPlaneFromPointNormal = (APIWRAPPER)GetProcAddress(h_original, "D3DXPlaneFromPointNormal");
    p_D3DXPlaneFromPoints = (APIWRAPPER)GetProcAddress(h_original, "D3DXPlaneFromPoints");
    p_D3DXPlaneIntersectLine = (APIWRAPPER)GetProcAddress(h_original, "D3DXPlaneIntersectLine");
    p_D3DXPlaneNormalize = (APIWRAPPER)GetProcAddress(h_original, "D3DXPlaneNormalize");
    p_D3DXPlaneTransform = (APIWRAPPER)GetProcAddress(h_original, "D3DXPlaneTransform");
    p_D3DXPlaneTransformArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXPlaneTransformArray");
    p_D3DXPreprocessShader = (APIWRAPPER)GetProcAddress(h_original, "D3DXPreprocessShader");
    p_D3DXPreprocessShaderFromFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXPreprocessShaderFromFileA");
    p_D3DXPreprocessShaderFromFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXPreprocessShaderFromFileW");
    p_D3DXPreprocessShaderFromResourceA = (APIWRAPPER)GetProcAddress(h_original, "D3DXPreprocessShaderFromResourceA");
    p_D3DXPreprocessShaderFromResourceW = (APIWRAPPER)GetProcAddress(h_original, "D3DXPreprocessShaderFromResourceW");
    p_D3DXQuaternionBaryCentric = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionBaryCentric");
    p_D3DXQuaternionExp = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionExp");
    p_D3DXQuaternionInverse = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionInverse");
    p_D3DXQuaternionLn = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionLn");
    p_D3DXQuaternionMultiply = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionMultiply");
    p_D3DXQuaternionNormalize = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionNormalize");
    p_D3DXQuaternionRotationAxis = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionRotationAxis");
    p_D3DXQuaternionRotationMatrix = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionRotationMatrix");
    p_D3DXQuaternionRotationYawPitchRoll = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionRotationYawPitchRoll");
    p_D3DXQuaternionSlerp = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionSlerp");
    p_D3DXQuaternionSquad = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionSquad");
    p_D3DXQuaternionSquadSetup = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionSquadSetup");
    p_D3DXQuaternionToAxisAngle = (APIWRAPPER)GetProcAddress(h_original, "D3DXQuaternionToAxisAngle");
    p_D3DXRectPatchSize = (APIWRAPPER)GetProcAddress(h_original, "D3DXRectPatchSize");
    p_D3DXSHAdd = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHAdd");
    p_D3DXSHDot = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHDot");
    p_D3DXSHEvalConeLight = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHEvalConeLight");
    p_D3DXSHEvalDirection = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHEvalDirection");
    p_D3DXSHEvalDirectionalLight = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHEvalDirectionalLight");
    p_D3DXSHEvalHemisphereLight = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHEvalHemisphereLight");
    p_D3DXSHEvalSphericalLight = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHEvalSphericalLight");
    p_D3DXSHPRTCompSplitMeshSC = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHPRTCompSplitMeshSC");
    p_D3DXSHPRTCompSuperCluster = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHPRTCompSuperCluster");
    p_D3DXSHProjectCubeMap = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHProjectCubeMap");
    p_D3DXSHRotate = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHRotate");
    p_D3DXSHRotateZ = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHRotateZ");
    p_D3DXSHScale = (APIWRAPPER)GetProcAddress(h_original, "D3DXSHScale");
    p_D3DXSaveMeshHierarchyToFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveMeshHierarchyToFileA");
    p_D3DXSaveMeshHierarchyToFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveMeshHierarchyToFileW");
    p_D3DXSaveMeshToXA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveMeshToXA");
    p_D3DXSaveMeshToXW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveMeshToXW");
    p_D3DXSavePRTBufferToFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSavePRTBufferToFileA");
    p_D3DXSavePRTBufferToFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSavePRTBufferToFileW");
    p_D3DXSavePRTCompBufferToFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSavePRTCompBufferToFileA");
    p_D3DXSavePRTCompBufferToFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSavePRTCompBufferToFileW");
    p_D3DXSaveSurfaceToFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveSurfaceToFileA");
    p_D3DXSaveSurfaceToFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveSurfaceToFileInMemory");
    p_D3DXSaveSurfaceToFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveSurfaceToFileW");
    p_D3DXSaveTextureToFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveTextureToFileA");
    p_D3DXSaveTextureToFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveTextureToFileInMemory");
    p_D3DXSaveTextureToFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveTextureToFileW");
    p_D3DXSaveVolumeToFileA = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveVolumeToFileA");
    p_D3DXSaveVolumeToFileInMemory = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveVolumeToFileInMemory");
    p_D3DXSaveVolumeToFileW = (APIWRAPPER)GetProcAddress(h_original, "D3DXSaveVolumeToFileW");
    p_D3DXSimplifyMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXSimplifyMesh");
    p_D3DXSphereBoundProbe = (APIWRAPPER)GetProcAddress(h_original, "D3DXSphereBoundProbe");
    p_D3DXSplitMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXSplitMesh");
    p_D3DXTessellateNPatches = (APIWRAPPER)GetProcAddress(h_original, "D3DXTessellateNPatches");
    p_D3DXTessellateRectPatch = (APIWRAPPER)GetProcAddress(h_original, "D3DXTessellateRectPatch");
    p_D3DXTessellateTriPatch = (APIWRAPPER)GetProcAddress(h_original, "D3DXTessellateTriPatch");
    p_D3DXTriPatchSize = (APIWRAPPER)GetProcAddress(h_original, "D3DXTriPatchSize");
    p_D3DXUVAtlasCreate = (APIWRAPPER)GetProcAddress(h_original, "D3DXUVAtlasCreate");
    p_D3DXUVAtlasPack = (APIWRAPPER)GetProcAddress(h_original, "D3DXUVAtlasPack");
    p_D3DXUVAtlasPartition = (APIWRAPPER)GetProcAddress(h_original, "D3DXUVAtlasPartition");
    p_D3DXValidMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXValidMesh");
    p_D3DXValidPatchMesh = (APIWRAPPER)GetProcAddress(h_original, "D3DXValidPatchMesh");
    p_D3DXVec2BaryCentric = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2BaryCentric");
    p_D3DXVec2CatmullRom = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2CatmullRom");
    p_D3DXVec2Hermite = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2Hermite");
    p_D3DXVec2Normalize = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2Normalize");
    p_D3DXVec2Transform = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2Transform");
    p_D3DXVec2TransformArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2TransformArray");
    p_D3DXVec2TransformCoord = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2TransformCoord");
    p_D3DXVec2TransformCoordArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2TransformCoordArray");
    p_D3DXVec2TransformNormal = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2TransformNormal");
    p_D3DXVec2TransformNormalArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec2TransformNormalArray");
    p_D3DXVec3BaryCentric = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3BaryCentric");
    p_D3DXVec3CatmullRom = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3CatmullRom");
    p_D3DXVec3Hermite = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3Hermite");
    p_D3DXVec3Normalize = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3Normalize");
    p_D3DXVec3Project = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3Project");
    p_D3DXVec3ProjectArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3ProjectArray");
    p_D3DXVec3Transform = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3Transform");
    p_D3DXVec3TransformArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3TransformArray");
    p_D3DXVec3TransformCoord = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3TransformCoord");
    p_D3DXVec3TransformCoordArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3TransformCoordArray");
    p_D3DXVec3TransformNormal = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3TransformNormal");
    p_D3DXVec3TransformNormalArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3TransformNormalArray");
    p_D3DXVec3Unproject = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3Unproject");
    p_D3DXVec3UnprojectArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec3UnprojectArray");
    p_D3DXVec4BaryCentric = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4BaryCentric");
    p_D3DXVec4CatmullRom = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4CatmullRom");
    p_D3DXVec4Cross = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4Cross");
    p_D3DXVec4Hermite = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4Hermite");
    p_D3DXVec4Normalize = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4Normalize");
    p_D3DXVec4Transform = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4Transform");
    p_D3DXVec4TransformArray = (APIWRAPPER)GetProcAddress(h_original, "D3DXVec4TransformArray");
	p_D3DXWeldVertices = (APIWRAPPER)GetProcAddress(h_original, "D3DXWeldVertices");
}