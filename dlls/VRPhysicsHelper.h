
#pragma once

#include <vector>
#include <unordered_map>

namespace reactphysics3d
{
	class CollisionWorld;
	class DynamicsWorld;
	class BoxShape;
	class CapsuleShape;
	class CollisionBody;
	class RigidBody;
	class TriangleVertexArray;
	class TriangleMesh;
	class ConcaveMeshShape;
	class ConvexMeshShape;
	class ProxyShape;
	class PolygonVertexArray;
	class PolyhedronMesh;
	class SphereShape;
	struct Vector3;
};  // namespace reactphysics3d

class CWorld;
class CBaseEntity;

struct VRPhysicsHelperModelBBoxIntersectResult
{
	Vector hitpoint;
	int hitgroup{ 0 };
	bool hasresult{ false };
};

class VRPhysicsHelper
{
public:
	VRPhysicsHelper();
	~VRPhysicsHelper();

	static void CreateInstance();
	static void DestroyInstance();
	static VRPhysicsHelper& Instance();

	// Called by engine every frame
	inline void StartFrame()
	{
		// Make sure we have an up-to-date model of the world!
		CheckWorld();
	}

	bool CheckIfLineIsBlocked(const Vector& hlPos1, const Vector& hlPos2);
	bool CheckIfLineIsBlocked(const Vector& pos1, const Vector& pos2, Vector& result);

	Vector RotateVectorInline(const Vector& vecToRotate, const Vector& vecAngles, const Vector& vecOffset = Vector(), const bool reverse = false);
	void RotateVector(Vector& vecToRotate, const Vector& vecAngles, const Vector& vecOffset = Vector(), const bool reverse = false);
	Vector AngularVelocityToLinearVelocity(const Vector& avelocity, const Vector& pos);

	void TraceLine(const Vector& vecStart, const Vector& vecEnd, edict_t* pentIgnore, TraceResult* ptr);

	bool ModelIntersectsBBox(CBaseEntity* pModel, const Vector& bboxCenter, const Vector& bboxMins, const Vector& bboxMaxs, const Vector& bboxAngles = Vector{}, VRPhysicsHelperModelBBoxIntersectResult* result = nullptr);
	bool ModelIntersectsCapsule(CBaseEntity* pModel, const Vector& capsuleCenter, double radius, double height);

	bool ModelBBoxIntersectsBBox(
		const Vector& bboxCenter1, const Vector& bboxMins1, const Vector& bboxMaxs1, const Vector& bboxAngles1, const Vector& bboxCenter2, const Vector& bboxMins2, const Vector& bboxMaxs2, const Vector& bboxAngles2, VRPhysicsHelperModelBBoxIntersectResult* result = nullptr);

	bool GetBSPModelBBox(CBaseEntity* pModel, Vector* bboxMins, Vector* bboxMaxs, Vector* bboxCenter = nullptr);

	// easter egg, see CWorldsSmallestCup
	void SetWorldsSmallestCupPosition(CBaseEntity* pWorldsSmallestCup);
	void GetWorldsSmallestCupPosition(CBaseEntity* pWorldsSmallestCup);


	class HitBoxModelData
	{
	public:
		~HitBoxModelData();

		void CreateData(reactphysics3d::CollisionWorld* collisionWorld, const Vector& origin, const Vector& mins, const Vector& maxs, const Vector& angles);
		void DeleteData();
		void UpdateTransform(const Vector& origin, const Vector& angles);
		bool HasData() { return m_hasData; }

		Vector m_mins;
		Vector m_maxs;
		Vector m_center;

		reactphysics3d::BoxShape* m_bboxShape{ nullptr };
		reactphysics3d::ProxyShape* m_proxyShape{ nullptr };
		reactphysics3d::CollisionBody* m_collisionBody{ nullptr };

		reactphysics3d::CollisionWorld* m_collisionWorld{ nullptr };

		bool m_hasData{ false };
	};

	class BSPModelData
	{
	public:
		~BSPModelData();

		void CreateData(reactphysics3d::CollisionWorld* collisionWorld);
		void DeleteData();
		bool HasData() { return m_hasData; }

		std::vector<struct reactphysics3d::Vector3> m_vertices;
		std::vector<int32_t> m_indices;

		reactphysics3d::TriangleVertexArray* m_triangleVertexArray{ nullptr };
		reactphysics3d::TriangleMesh* m_triangleMesh{ nullptr };
		reactphysics3d::ConcaveMeshShape* m_concaveMeshShape{ nullptr };
		reactphysics3d::ProxyShape* m_proxyShape{ nullptr };
		reactphysics3d::CollisionBody* m_collisionBody{ nullptr };

		reactphysics3d::CollisionWorld* m_collisionWorld{ nullptr };

		bool m_hasData{ false };
	};

	class DynamicBSPModelData
	{
	public:
		~DynamicBSPModelData();

		void CreateData(reactphysics3d::DynamicsWorld* dynamicsWorld);
		void DeleteData();
		bool HasData() { return m_hasData; }

		std::vector<struct reactphysics3d::Vector3> m_vertices;
		std::vector<struct reactphysics3d::Vector3> m_normals;
		std::vector<int32_t> m_indices;

		reactphysics3d::TriangleVertexArray* m_triangleVertexArray{ nullptr };
		reactphysics3d::TriangleMesh* m_triangleMesh{ nullptr };
		reactphysics3d::ConcaveMeshShape* m_concaveMeshShape{ nullptr };
		reactphysics3d::ProxyShape* m_proxyShape{ nullptr };
		reactphysics3d::RigidBody* m_rigidBody{ nullptr };

		reactphysics3d::DynamicsWorld* m_dynamicsWorld{ nullptr };

		bool m_hasData{ false };
	};

private:
	struct HitBox
	{
		//Vector origin;
		//Vector angles;
		Vector mins;
		Vector maxs;
		class Hash
		{
		public:
			std::size_t operator()(const HitBox& hitbox) const
			{
				std::hash<float> floatHasher;
				return
					//floatHasher(hitbox.origin.x) ^ floatHasher(hitbox.origin.y) ^ floatHasher(hitbox.origin.z)
					//^ floatHasher(hitbox.angles.x) ^ floatHasher(hitbox.angles.y) ^ floatHasher(hitbox.angles.z)
					//^
					floatHasher(hitbox.mins.x) ^ floatHasher(hitbox.mins.y) ^ floatHasher(hitbox.mins.z) ^ floatHasher(hitbox.maxs.x) ^ floatHasher(hitbox.maxs.y) ^ floatHasher(hitbox.maxs.z);
			}
		};
		class Equal
		{
		public:
			bool operator()(const HitBox& hitbox1, const HitBox& hitbox2) const
			{
				return  //hitbox1.origin == hitbox2.origin
					//&& hitbox1.angles == hitbox2.angles
					//&&
					hitbox1.mins == hitbox2.mins && hitbox1.maxs == hitbox2.maxs;
			}
		};
	};

	bool CheckWorld();

	void InitPhysicsWorld();

	bool GetPhysicsMapDataFromFile(const std::string& mapFilePath, const std::string& physicsMapDataFilePath);
	void StorePhysicsMapDataToFile(const std::string& mapFilePath, const std::string& physicsMapDataFilePath);
	void GetPhysicsMapDataFromModel();

	void EnsureWorldsSmallestCupExists(CBaseEntity* pWorldsSmallestCup);

	reactphysics3d::CollisionBody* GetHitBoxBody(size_t cacheIndex, const Vector& bboxCenter, const Vector& bboxMins, const Vector& bboxMaxs, const Vector& bboxAngles = Vector{});

	bool TestCollision(reactphysics3d::CollisionBody* body1, reactphysics3d::CollisionBody* body2, VRPhysicsHelperModelBBoxIntersectResult* result = nullptr);

	std::string m_currentMapName;
	std::unordered_map<std::string, BSPModelData> m_bspModelData;
	std::unordered_map<std::string, DynamicBSPModelData> m_dynamicBSPModelData;

	std::vector<std::unordered_map<HitBox, std::shared_ptr<HitBoxModelData>, HitBox::Hash, HitBox::Equal>> m_hitboxCaches;

	const struct model_s* m_hlWorldModel{ nullptr };
	reactphysics3d::CollisionWorld* m_collisionWorld{ nullptr };
	reactphysics3d::DynamicsWorld* m_dynamicsWorld{ nullptr };

	/*
	reactphysics3d::CollisionBody*							m_bboxBody1{ nullptr };
	reactphysics3d::BoxShape*								m_bboxShape1{ nullptr };
	reactphysics3d::ProxyShape*								m_bboxProxyShape1{ nullptr };

	reactphysics3d::CollisionBody*							m_bboxBody2{ nullptr };
	reactphysics3d::BoxShape*								m_bboxShape2{ nullptr };
	reactphysics3d::ProxyShape*								m_bboxProxyShape2{ nullptr };
	*/

	reactphysics3d::CollisionBody* m_capsuleBody{ nullptr };
	reactphysics3d::CapsuleShape* m_capsuleShape{ nullptr };
	reactphysics3d::ProxyShape* m_capsuleProxyShape{ nullptr };

	// can't forward declare inner type so we use void*
	void* m_worldsSmallestCupPolygonFaces{ nullptr };
	reactphysics3d::PolygonVertexArray* m_worldsSmallestCupPolygonVertexArray{ nullptr };
	reactphysics3d::PolyhedronMesh* m_worldsSmallestCupPolyhedronMesh{ nullptr };
	reactphysics3d::RigidBody* m_worldsSmallestCupBody{ nullptr };
	reactphysics3d::SphereShape* m_worldsSmallestCupTopSphereShape{ nullptr };
	reactphysics3d::SphereShape* m_worldsSmallestCupBottomSphereShape{ nullptr };
	reactphysics3d::ProxyShape* m_worldsSmallestCupTopProxyShape{ nullptr };
	reactphysics3d::ProxyShape* m_worldsSmallestCupBottomProxyShape{ nullptr };
	float vertices[24] = { 0 };
	int indices[24] = { 0 };

	static VRPhysicsHelper* m_instance;
};
