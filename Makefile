define default
$(if $(1),$(1),$(2))
endef

define prefix
$(if $(1),$(2)$(1),)
endef

up:
	@docker compose up -d ${up-with}

down:
	@docker compose down ${down-with}

restart:
	${MAKE} down up

shell:
	@docker compose exec $(call default,${service},database) bash

ps:
	@docker compose ps
status: ps

logs:
	@docker compose logs $(call default,${service},database) ${with}

test:
	@dotnet test $(call default,$(call prefix,${filter},--filter=),$(call prefix,${f},--filter=)) ${with}
