from neo_fairy_client import FairyClient, Hash160Str  # 3.5.0.10

client = FairyClient(wallet_address_or_scripthash=Hash160Str.from_address('Nb2CHYY5wTh2ac58mTue5S3wpG6bQv5hSY'), fairy_session='RandLib', with_print=False)
print(client.virutal_deploy_from_path('./bin/sc/RandLib.nef'))
nef, manifest = client.get_nef_and_manifest_from_path('./bin/sc/RandLib.nef')
client.invokefunction('update', [nef, manifest])
assert client.invokefunction('uint128') < 2**128
print(client.invokefunction('bits', [0]))
print(client.invokefunction('bits', [1]))
print(client.invokefunction('bits', [9]))
print(r := client.invokefunction('bits', [129]), len(r))
print(client.invokefunction('range', [0, 2]))
print(client.invokefunction('range', [-10, 100]))
print(client.invokefunction('choice', [[-10, 100, "a", Hash160Str('0xb1983fa2479a0c8e2beae032d2df564b5451b7a5')]]))
print(client.invokefunction('shuffle', [[-10, 100, "a", Hash160Str('0xb1983fa2479a0c8e2beae032d2df564b5451b7a5')]]))
print(client.invokefunction('randChoices', [[-10, 100, "a", Hash160Str('0xb1983fa2479a0c8e2beae032d2df564b5451b7a5')], 2]))