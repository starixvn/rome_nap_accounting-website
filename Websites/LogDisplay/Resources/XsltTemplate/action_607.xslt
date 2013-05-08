<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupdmovemaster">
    <table class="tblDesc">
      <tr>
        <td class="colW1">{lbl:masterName}</td>
        <td class="colW2">
          <xsl:value-of select="@mastername" />
        </td>

        <td class="colW1">{lbl:newSuperName}</td>
        <td class="colW2">
          <xsl:value-of select="@newsupername" />
        </td>
        <td class="colW1">{lbl:ip}</td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>

      </tr>
      <tr>
        <td class="colW1">{lbl:errCode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">{lbl:errMsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <xsl:choose>
          <xsl:when test="$IsInternal = 0">
            <td class="colW1"></td>
            <td class="colW2">

            </td>

          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">{lbl:oldSuperId}</td>
            <td class="colW2">
              <xsl:value-of select="@oldsuperid" />
            </td>


          </xsl:otherwise>
        </xsl:choose>

      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">
        </xsl:when>
        <xsl:otherwise>
          <tr>

            <td class="colW1">{lbl:newSuperId}</td>
            <td class="colW2">
              <xsl:value-of select="@newsuperid" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>

        </xsl:otherwise>
      </xsl:choose>



    </table>


  </xsl:template>
</xsl:stylesheet>
