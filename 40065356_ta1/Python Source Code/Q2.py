import numpy as np

c1_asp = np.array([22, 18])
c1_ap = np.array([21, 16])
c1_av = np.array([3, 1])

c2_asp = np.array([6, 13])
c2_ap = np.array([5, 11])
c2_av = np.array([3, 3])

c3_asp = np.array([29, 12])
c3_ap = np.array([28, 9])
c3_av = np.array([6, 5])

k_offset = 1

pc = (c1_ap + c2_ap + c3_ap)/3
print('pc = ' + str(pc))
vc = (c1_av + c2_av + c3_av)/3
print('vc = ' + str(vc))
p_anchor = pc + (k_offset * vc)
print('p_anchor = ' + str(p_anchor))

print()

delta_ps1 = c1_asp - pc
print("delta_ps1 = " + str(delta_ps1))
pc1 = p_anchor + delta_ps1
print("pc1 = " + str(pc1))

print()

delta_ps2 = c2_asp - pc
print("delta_ps2 = " + str(delta_ps2))
pc2 = p_anchor + delta_ps2
print("pc2 = " + str(pc2))

print()

delta_ps3 = c3_asp - pc
print("delta_ps3 = " + str(delta_ps3))
pc3 = p_anchor + delta_ps3
print("pc3 = " + str(pc3))

print()

pc = (c1_ap + c2_ap)/2
print(pc)
vc = (c1_av + c2_av)/2
print(vc)
p_anchor = pc + (k_offset * vc)
print('p_anchor = ' + str(p_anchor))

print()

delta_ps1 = c1_asp - pc
print("delta_ps1 = " + str(delta_ps1))
pc1 = p_anchor + delta_ps1
print("pc1 = " + str(pc1))
print()
delta_ps2 = c2_asp - pc
print("delta_ps2 = " + str(delta_ps2))
pc2 = p_anchor + delta_ps2
print("pc2 = " + str(pc2))

